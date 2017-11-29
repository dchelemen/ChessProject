using ChessTable.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using ChessTable.Model.Rules;
using ChessTable.Model.Algorithms;
using System.Timers;

namespace ChessTable.Model
{
	public class ChessBoardModel
	{
		public ChessBoardModel( Colors aPlayer1Color, Colors aStartingColor, Algorithm aPlayer1Algorithm, Algorithm aPlayer2Algorithm )
		{
			mPlayer1Color		= aPlayer1Color;
			Colors player2Color	= ( aPlayer1Color == Colors.WHITE ? Colors.BLACK : Colors.WHITE );
			mPlayer1Algorithm	= setAlgorithm( aPlayer1Algorithm, mPlayer1Color );
			mPlayer2Algorithm	= setAlgorithm( aPlayer2Algorithm, player2Color );
			mCurrentColor		= aStartingColor;
			nextPlayer			= new EventHandler< Move >( onNextPlayer );
			mTimer				= new Timer();
			mTimer.Elapsed		+= new ElapsedEventHandler( onTimerFinished );
			mTimer.Interval		= 100;

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
			mCastlingRule		= new CastlingRule();
			mEnPassantRule		= new EnPassantRule();
			mChessRule			= new ChessRule();
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

			mPlayer1Algorithm.setTree( chessBoard, whiteFigures, blackFigures, mCastlingRule, mChessRule );
			mPlayer2Algorithm.setTree( chessBoard, whiteFigures, blackFigures, mCastlingRule, mChessRule );

			mLastMove = null;
			mTimer.Start();
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private void onNextPlayer( Object aSender, Move aLastMove = null )
		{
			if ( mIsBoardEnabled )
			{
				mIsBoardEnabled = false;
				setIsEnable( this, mIsBoardEnabled );
			}

			BaseAlgorithm currentAlgorithm = ( mCurrentColor == mPlayer1Color ? mPlayer1Algorithm : mPlayer2Algorithm );

			if ( ! currentAlgorithm.isActive ) // Human
			{
				mIsBoardEnabled = true;
				setIsEnable( this, mIsBoardEnabled );
			}
			else // Algorithm
			{
				currentAlgorithm.refreshTree( chessBoard, whiteFigures, blackFigures, mCastlingRule, aLastMove, mChessRule );
				Move nextMove = currentAlgorithm.nextMove( chessBoard, whiteFigures, blackFigures, mCastlingRule );
				mFigureToMove = nextMove.itemFrom;

				moveFigureTo( nextMove.itemTo );
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

			possibleMoves = getPossibleMoves( mFigureToMove );

			setHighlight( this, new SetHighlightEventArg
			{
				index = aFigureToMove.index,
				color = Colors.BLUE
			} );

			foreach ( Int32 fields in possibleMoves )
			{
				setHighlight( this, new SetHighlightEventArg
				{
					index = fields,
					color = Colors.BLUE
				} );
			}

			mIsFirstClick = false;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private void moveFigureTo( ModelItem aPlaceHere )
		{
			if ( mChessRule.isChess )
			{
				mChessRule.resetChess(); 
			}

			if ( mCastlingRule.isCastling( mFigureToMove, aPlaceHere ) )
			{
				castling( aPlaceHere );
				removeHighLights();
				mLastMove = new Move( mFigureToMove, chessBoard[ aPlaceHere.x ][ aPlaceHere.y ] );
				mTimer.Start();
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

			mLastMove = new Move( mFigureToMove, chessBoard[ aPlaceHere.x ][ aPlaceHere.y ] );

			ModelItem lastItem = chessBoard[ aPlaceHere.x ][ aPlaceHere.y ];
			List< Int32 > posMoves = getPossibleMoves( new ModelItem( lastItem.x, lastItem.y, lastItem.figureItem.color, lastItem.figureItem.figureType ) );
			foreach ( Int32 index in posMoves )
			{
				ModelItem item = chessBoard[ index / 8 ][ index % 8 ];
				if ( item.figureItem.figureType == FigureType.KING && lastItem.figureItem.color != item.figureItem.color )
				{
					mChessRule.setChess( index, lastItem, posMoves );
					break;
				}
			}
			mTimer.Start();
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

		private List< Int32 > getPossibleMoves( ModelItem aFigureToMove )
		{
			List< Int32 > pMoves = new List< Int32 >();
			switch ( aFigureToMove.figureItem.figureType )
			{
			case FigureType.KING:
				{
					KingRule kingRule		= new KingRule( chessBoard, mPlayer1Color, aFigureToMove, blackFigures, whiteFigures, mCastlingRule );
					pMoves			= kingRule.possibleMoves( mChessRule );
				} break;
			case FigureType.QUEEN:
				{
					QueenRule queenRule		= new QueenRule( chessBoard, mPlayer1Color, aFigureToMove );
					pMoves			= queenRule.possibleMoves( mChessRule );
				} break;
			case FigureType.ROOK:
				{
					RookRule rookRule		= new RookRule( chessBoard, mPlayer1Color, aFigureToMove );
					pMoves			= rookRule.possibleMoves( mChessRule );
				} break;
			case FigureType.BISHOP:
				{
					BishopRule bishopRule	= new BishopRule( chessBoard, mPlayer1Color, aFigureToMove );
					pMoves			= bishopRule.possibleMoves( mChessRule );
				} break;
			case FigureType.KNIGHT:
				{
					KnightRule knightRule	= new KnightRule( chessBoard, mPlayer1Color, aFigureToMove );
					pMoves			= knightRule.possibleMoves( mChessRule );
				} break;
			case FigureType.PAWN:
				{
					PawnRule pawnRule		= new PawnRule( chessBoard, mPlayer1Color, aFigureToMove );
					pMoves			= pawnRule.possibleMoves( mChessRule );
				} break;
			case FigureType.NO_FIGURE:		break;
			}

			return pMoves;
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

			ModelItem lastItem = chessBoard[ newRookPlace.x ][ aPlaceHere.y + moveYCoordForRookBy ];
			List< Int32 > posMoves = getPossibleMoves( new ModelItem( lastItem.x, lastItem.y, lastItem.figureItem.color, lastItem.figureItem.figureType ) );
			foreach ( Int32 index in posMoves )
			{
				ModelItem item = chessBoard[ index / 8 ][ index % 8 ];
				if ( item.figureItem.figureType == FigureType.KING && lastItem.figureItem.color != item.figureItem.color )
				{
					mChessRule.setChess( index, lastItem, posMoves );
					break;
				}
			}

			removeHighLights();
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public void disableCastling()
		{
			mCastlingRule.disable();
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private BaseAlgorithm setAlgorithm( Algorithm aAlgorithm, Colors aAlgorithmsColor )
		{
			switch ( aAlgorithm )
			{
			case Algorithm.HUMAN: return new HumanAlgorithm( mPlayer1Color, aAlgorithmsColor );
			case Algorithm.RANDOM: return new RandomAlgorithm( mPlayer1Color, aAlgorithmsColor );
			default: return new HumanAlgorithm( mPlayer1Color, aAlgorithmsColor );
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private void onTimerFinished( object source, ElapsedEventArgs e )
		{
			mTimer.Stop();
			nextPlayer( this, mLastMove );
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public event EventHandler< PutFigureOnTheTableEventArg >	fieldClicked;

		public event EventHandler< SetHighlightEventArg >			setHighlight;

		public event EventHandler< Move >							nextPlayer;

		public event EventHandler< Boolean >						setIsEnable;

		public Boolean												isGameReady { get; set; }
		public List< ModelItem >									whiteFigures { get; set; }
		public List< ModelItem >									blackFigures { get; set; }

		private List< Int32 >										possibleMoves { get; set; }
		private List< List< ModelItem > >							chessBoard { get; set; }

		//-----------------------------------------------------------------------------------------------------------------------------------------

		private ChessRule											mChessRule;
		private Move												mLastMove;
		private Colors												mPlayer1Color;
		private BaseAlgorithm										mPlayer1Algorithm;
		private BaseAlgorithm										mPlayer2Algorithm;
		private Colors												mCurrentColor;

		private Boolean												mIsFirstClick;
		private Boolean												mIsBoardEnabled;

		private ModelItem											mFigureToMove;
		private CastlingRule										mCastlingRule;
		private EnPassantRule										mEnPassantRule;
		private Timer												mTimer;
	}
}
