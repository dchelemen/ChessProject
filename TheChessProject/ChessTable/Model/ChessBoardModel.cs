using ChessTable.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using ChessTable.Model.Rules;
using ChessTable.Model.Algorithms;

namespace ChessTable.Model
{
	public class ChessBoardModel
	{
		public ChessBoardModel( Colors aPlayer1Color, Colors aStartingColor, Algorithm aPlayer1Algorithm, Algorithm aPlayer2Algorithm )
		{
			mPlayer1Color		= aPlayer1Color;
			mPlayer1Algorithm	= setAlgorithm( aPlayer1Algorithm );
			mPlayer2Algorithm	= setAlgorithm( aPlayer1Algorithm );
			mCurrentColor		= aStartingColor;

			whiteFigures		= new List< ModelItem >();
			blackFigures		= new List< ModelItem >();

			isGameReady			= false;
			mIsFirstClick		= true;
			mIsBoardEnabled		= false;

			mFigureToMove		= new ModelItem
								{
									index		= -1,
									figureItem	= new FigureItem( Colors.NO_COLOR, FigureType.NO_FIGURE ),
									x			= -1,
									y			= -1
								};
			mCastlingRule	= new CastlingRule();
			mEnPassantRule	= new EnPassantRule();
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public void startModel()
		{
			chessBoard		= new List< List< ModelItem > >();
			possibleMoves	= new List< Int32 >();

			for ( Int32 row = 0; row < 8; row++ )
			{
				List< ModelItem > chessBoardRow = new List< ModelItem >();
				for ( Int32 column = 0; column < 8; column++ )
				{
					chessBoardRow.Add( new ModelItem
					{
						index		= row * 8 + column,
						x			= row,
						y			= column,
						figureItem	= new FigureItem( Colors.NO_COLOR, FigureType.NO_FIGURE )
					} );
				}
				chessBoard.Add( chessBoardRow );
			}

			foreach ( ModelItem item in whiteFigures )
			{
				chessBoard[ item.x ][ item.y ] = new ModelItem( item.x, item.y, item.figureItem.color, item.figureItem.figureType );
				fieldClicked( this, new PutFigureOnTheTableEventArg
				{
					figureItem	= item.figureItem,
					x			= item.x,
					y			= item.y,
					index		= item.index
				} );
			}

			foreach ( ModelItem item in blackFigures )
			{
				chessBoard[ item.x ][ item.y ] = new ModelItem( item.x, item.y, item.figureItem.color, item.figureItem.figureType );
				fieldClicked( this, new PutFigureOnTheTableEventArg
				{
					figureItem	= item.figureItem,
					x			= item.x,
					y			= item.y,
					index		= item.index
				} );
			}

			if ( mPlayer1Color == Colors.WHITE )
			{
				mPlayer1Algorithm.setTree( whiteFigures, blackFigures );
				mPlayer2Algorithm.setTree( blackFigures, whiteFigures );
			}
			else
			{
				mPlayer1Algorithm.setTree( blackFigures, whiteFigures );
				mPlayer2Algorithm.setTree( whiteFigures, blackFigures );
			}

			nextPlayer();
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private void nextPlayer( Move aLastMove = null )
		{
			if ( mIsBoardEnabled )
			{
				mIsBoardEnabled = false;
				setIsEnable( this, mIsBoardEnabled );
			}

			BaseAlgorithm currentAlgorithm = ( mCurrentColor == mPlayer1Color ? mPlayer1Algorithm : mPlayer2Algorithm );

			Boolean isActiveAlgorithm = currentAlgorithm.isActive();
			if ( ! isActiveAlgorithm )
			{
				mIsBoardEnabled = true;
				setIsEnable( this, mIsBoardEnabled );
			}
			else
			{
				currentAlgorithm.refreshTree( aLastMove );
				Move nextMove = currentAlgorithm.nextMove();

				ModelItem figureToMove		= chessBoard[ nextMove.mMoveFromX ][ nextMove.mMoveFromY ];
				ModelItem placeHereTemp		= chessBoard[ nextMove.mMoveToX ][ nextMove.mMoveToY ];

				mFigureToMove = new ModelItem( figureToMove.x, figureToMove.y, figureToMove.figureItem.color, figureToMove.figureItem.figureType );

				moveFigureTo( new ModelItem( placeHereTemp.x, placeHereTemp.y, placeHereTemp.figureItem.color, placeHereTemp.figureItem.figureType ) );
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public void moveFigure( ModelItem aIndex )
		{
			if ( mIsFirstClick )
			{
				moveFigureFrom( aIndex );
			}
			else
			{
				if ( aIndex == mFigureToMove )
				{
					removeHighLights();
					mIsFirstClick = true;
					return;
				}

				if ( ! possibleMoves.Contains( aIndex.index ) )
				{
					return;
				}

				mIsFirstClick = true;
				moveFigureTo( aIndex );
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private void moveFigureFrom( ModelItem aFigureToMove )
		{
			if ( aFigureToMove.figureItem.color != mCurrentColor )
			{
				return;
			}

			mFigureToMove = aFigureToMove;

			setPossibleMoves();

			setHighlight( this, new SetHighlightEventArg
			{
				index = aFigureToMove.index,
				color = Colors.BLUE
			} );

			mIsFirstClick = false;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private void moveFigureTo( ModelItem aPlaceHere )
		{
			if ( mCastlingRule.isCastling( mFigureToMove, aPlaceHere ) )
			{
				castling( aPlaceHere );
				removeHighLights();
				return;
			}

			mCastlingRule.updateCastlingState( mFigureToMove );
			if ( mEnPassantRule.isEnPassantActive && aPlaceHere.index == mEnPassantRule.temporaryPawn.index && mFigureToMove.figureItem.figureType == FigureType.PAWN )
			{
				removeFigureFromWhiteOrBlack( mEnPassantRule.originalPawn );
				fieldClicked( this, new PutFigureOnTheTableEventArg( mEnPassantRule.originalPawn.x, mEnPassantRule.originalPawn.y, mEnPassantRule.originalPawn.index, Colors.NO_COLOR, FigureType.NO_FIGURE ) );
				mEnPassantRule.originalPawn.figureItem.color		= Colors.NO_COLOR;
				mEnPassantRule.originalPawn.figureItem.figureType	= FigureType.NO_FIGURE;
			}
			mEnPassantRule.setEnPassant( mFigureToMove, aPlaceHere, chessBoard );

			removeFigureFromWhiteOrBlack( aPlaceHere );

			ModelItem tempItem;
			if ( mCurrentColor == Colors.WHITE )
			{
				tempItem = whiteFigures.Where( X => X.index == mFigureToMove.index ).FirstOrDefault();
				mCurrentColor = Colors.BLACK;
			}
			else
			{
				tempItem = blackFigures.Where( X => X.index == mFigureToMove.index ).FirstOrDefault();
				mCurrentColor = Colors.WHITE;
			}

			tempItem.index	= aPlaceHere.index;
			tempItem.x		= aPlaceHere.x;
			tempItem.y		= aPlaceHere.y;

			fieldClicked( this, new PutFigureOnTheTableEventArg( mFigureToMove.x, mFigureToMove.y, mFigureToMove.index, Colors.NO_COLOR, FigureType.NO_FIGURE ) );
			chessBoard[ mFigureToMove.x ][ mFigureToMove.y ].figureItem = new FigureItem( Colors.NO_COLOR, FigureType.NO_FIGURE );

			fieldClicked( this, new PutFigureOnTheTableEventArg( aPlaceHere.x, aPlaceHere.y, aPlaceHere.index, mFigureToMove.figureItem.color, mFigureToMove.figureItem.figureType ) );
			chessBoard[ aPlaceHere.x ][ aPlaceHere.y ].figureItem = new FigureItem( mFigureToMove.figureItem.color, mFigureToMove.figureItem.figureType );

			removeHighLights();
			nextPlayer();
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private void removeHighLights()
		{
			setHighlight( this, new SetHighlightEventArg // Removing HighLights from the Figure itself
			{
				index = mFigureToMove.index,
				color = Colors.NO_COLOR
			} );

			foreach ( Int32 fields in possibleMoves ) // Removing HighLights from the possible moves
			{
				setHighlight( this, new SetHighlightEventArg
				{
					index = fields,
					color = Colors.NO_COLOR
				} );
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private void setPossibleMoves()
		{
			possibleMoves.Clear();
			switch ( mFigureToMove.figureItem.figureType )
			{
			case FigureType.KING:
				{
					KingRule kingRule		= new KingRule( chessBoard, mPlayer1Color, mFigureToMove, blackFigures, whiteFigures, mCastlingRule );
					possibleMoves			= kingRule.possibleMoves();
				} break;
			case FigureType.QUEEN:
				{
					QueenRule queenRule		= new QueenRule( chessBoard, mPlayer1Color, mFigureToMove );
					possibleMoves			= queenRule.possibleMoves();
				} break;
			case FigureType.ROOK:
				{
					RookRule rookRule		= new RookRule( chessBoard, mPlayer1Color, mFigureToMove );
					possibleMoves			= rookRule.possibleMoves();
				} break;
			case FigureType.BISHOP:
				{
					BishopRule bishopRule	= new BishopRule( chessBoard, mPlayer1Color, mFigureToMove );
					possibleMoves			= bishopRule.possibleMoves();
				} break;
			case FigureType.KNIGHT:
				{
					KnightRule knightRule	= new KnightRule( chessBoard, mPlayer1Color, mFigureToMove );
					possibleMoves			= knightRule.possibleMoves();
				} break;
			case FigureType.PAWN:
				{
					PawnRule pawnRule		= new PawnRule( chessBoard, mPlayer1Color, mFigureToMove );
					possibleMoves			= pawnRule.possibleMoves();
				} break;
			case FigureType.NO_FIGURE:		break;
			}

			foreach ( Int32 fields in possibleMoves )
			{
				setHighlight( this, new SetHighlightEventArg
				{
					index = fields,
					color = Colors.BLUE
				} );
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		void removeFigureFromWhiteOrBlack( ModelItem aFigureToRemove )
		{
			if ( aFigureToRemove.figureItem.color == Colors.WHITE )
			{
				whiteFigures.Remove( aFigureToRemove );
			}
			else if ( aFigureToRemove.figureItem.color == Colors.BLACK )
			{
				blackFigures.Remove( aFigureToRemove );
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private void castling( ModelItem aPlaceHere )
		{
			ModelItem newKingPlace;
			ModelItem newRookPlace;
			ModelItem leftRook;
			ModelItem rightRook;

			if ( mCurrentColor == Colors.WHITE )
			{
				rightRook		= whiteFigures.Where( X => X.y == 7 && X.figureItem.figureType == FigureType.ROOK ).FirstOrDefault();
				leftRook		= whiteFigures.Where( X => X.y == 0 && X.figureItem.figureType == FigureType.ROOK ).FirstOrDefault();
				newKingPlace	= whiteFigures.Where( X => X.index == mFigureToMove.index ).FirstOrDefault();
				newRookPlace	= ( aPlaceHere.y > 4 ? rightRook : leftRook );
				mCurrentColor	= Colors.BLACK;
			}
			else
			{
				rightRook		= blackFigures.Where( X => X.y == 7 && X.figureItem.figureType == FigureType.ROOK ).FirstOrDefault();
				leftRook		= blackFigures.Where( X => X.y == 0 && X.figureItem.figureType == FigureType.ROOK ).FirstOrDefault();
				newKingPlace	= blackFigures.Where( X => X.index == mFigureToMove.index ).FirstOrDefault();
				newRookPlace	= ( aPlaceHere.y > 4 ? rightRook : leftRook );
				mCurrentColor	= Colors.WHITE;
			}
			Int32 moveYCoordForRookBy = ( newRookPlace == leftRook ? 1 : -1 );

			// move the King

			newKingPlace.index	= aPlaceHere.index;
			newKingPlace.x		= aPlaceHere.x;
			newKingPlace.y		= aPlaceHere.y;

			fieldClicked( this, new PutFigureOnTheTableEventArg( mFigureToMove.x, mFigureToMove.y, mFigureToMove.index, Colors.NO_COLOR, FigureType.NO_FIGURE ) );
			chessBoard[ mFigureToMove.x ][ mFigureToMove.y ].figureItem = new FigureItem( Colors.NO_COLOR, FigureType.NO_FIGURE );

			fieldClicked( this, new PutFigureOnTheTableEventArg( aPlaceHere.x, aPlaceHere.y, aPlaceHere.index, mFigureToMove.figureItem.color, mFigureToMove.figureItem.figureType ) );
			chessBoard[ aPlaceHere.x ][ aPlaceHere.y ].figureItem = new FigureItem( mFigureToMove.figureItem.color, mFigureToMove.figureItem.figureType );

			// move the Rook

			fieldClicked( this, new PutFigureOnTheTableEventArg( newRookPlace.x, newRookPlace.y, newRookPlace.index, Colors.NO_COLOR, FigureType.NO_FIGURE ) );
			chessBoard[ newRookPlace.x ][ newRookPlace.y ].figureItem = new FigureItem( Colors.NO_COLOR, FigureType.NO_FIGURE );

			fieldClicked( this, new PutFigureOnTheTableEventArg( newRookPlace.x, aPlaceHere.y + moveYCoordForRookBy, aPlaceHere.index + moveYCoordForRookBy,
																							newRookPlace.figureItem.color, newRookPlace.figureItem.figureType ) );
			chessBoard[ newRookPlace.x ][ aPlaceHere.y + moveYCoordForRookBy ].figureItem = new FigureItem( newRookPlace.figureItem.color, newRookPlace.figureItem.figureType );

			newRookPlace.index = aPlaceHere.index + moveYCoordForRookBy;
			newRookPlace.y = aPlaceHere.y + moveYCoordForRookBy;

			mCastlingRule.updateCastlingState( mFigureToMove );
			removeHighLights();
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public void disableCastling()
		{
			mCastlingRule.disable();
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private BaseAlgorithm setAlgorithm( Algorithm aAlgorithm )
		{
			switch ( aAlgorithm )
			{
			case Algorithm.HUMAN: return new HumanAlgorithm();
			default: return new HumanAlgorithm();
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public event EventHandler< PutFigureOnTheTableEventArg >	fieldClicked;

		public event EventHandler< SetHighlightEventArg >			setHighlight;

		public event EventHandler< Boolean >						setIsEnable;

		public Boolean												isGameReady { get; set; }
		public List< ModelItem >									whiteFigures { get; set; }
		public List< ModelItem >									blackFigures { get; set; }

		private List< Int32 >										possibleMoves { get; set; }
		private List< List< ModelItem > >							chessBoard { get; set; }

		//-----------------------------------------------------------------------------------------------------------------------------------------

		private Colors												mPlayer1Color;
		private BaseAlgorithm										mPlayer1Algorithm;
		private BaseAlgorithm										mPlayer2Algorithm;
		private Colors												mCurrentColor;

		private Boolean												mIsFirstClick;
		private Boolean												mIsBoardEnabled;

		private ModelItem											mFigureToMove;
		private CastlingRule										mCastlingRule;
		private EnPassantRule										mEnPassantRule;
	}
}
