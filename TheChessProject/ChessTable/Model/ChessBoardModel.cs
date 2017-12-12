using ChessTable.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using ChessTable.Model.Rules;
using ChessTable.Model.Algorithms;
using System.Timers;
using ChessTable.View;
using ChessTable.ViewModels;

namespace ChessTable.Model
{
	public class ChessBoardModel
	{
		public ChessBoardModel( Colors aPlayer1Color, Colors aStartingColor, Algorithm aPlayer1Algorithm, Algorithm aPlayer2Algorithm )
		{
			mFigureValues		= new FigureValues( aPlayer1Color, "FigureValues.xml" );
			mPlayer1Color		= aPlayer1Color;
			Colors player2Color	= ( aPlayer1Color == Colors.WHITE ? Colors.BLACK : Colors.WHITE );
			mPlayer1Algorithm	= setAlgorithm( aPlayer1Algorithm, mPlayer1Color );
			mPlayer2Algorithm	= setAlgorithm( aPlayer2Algorithm, player2Color );
			mCurrentColor		= aStartingColor;
			nextPlayer			= new EventHandler< Move >( onNextPlayer );
			mTimer				= new Timer();
			mTimer.Elapsed		+= new ElapsedEventHandler( onTimerFinished );
			mTimer.Interval		= mFigureValues.secondsBetweenMove;

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

			mLastMove = null;
			mTimer.Start();
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private void onNextPlayer( Object aSender, Move aLastMove = null )
		{
			Colors winner = Colors.NO_COLOR;
			Boolean isCheckmateOrDraw = isGameOver( ref winner );
			if ( isCheckmateOrDraw )
			{
				gameOver( this, winner );
				return;
			}
			
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
				currentAlgorithm.refreshTree( chessBoard, whiteFigures, blackFigures, aLastMove );
				Move lastMove = currentAlgorithm.move( chessBoard, whiteFigures, blackFigures );
				refreshBlackWhiteFigures( lastMove );

				mFigureToMove = lastMove.itemFrom;

				moveFigureTo( lastMove.itemTo );
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

				if ( mFigureToMove.figureItem.figureType == FigureType.PAWN && ( aIndex.x == 0 || aIndex.x == 7 ) )
				{
					ChooseFigureView chooseFigureView = new ChooseFigureView();
					chooseFigureView.ShowDialog();
					changeFigureToMoveType( chooseFigureView.selectedFigureType );
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

			possibleMoves = getPossibleMoves( mFigureToMove, true );

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
			if ( isCastling( mFigureToMove, aPlaceHere ) )
			{
				castling( aPlaceHere );
				removeHighLights();
				setLastMoveHighLights( mLastMove, false );
				mLastMove = new Move( mFigureToMove, chessBoard[ aPlaceHere.x ][ aPlaceHere.y ] );
				setLastMoveHighLights( mLastMove, true );
				mTimer.Start();
				return;
			}
			updateCastling();

			FigureItem enPassantFigure = chessBoard[ aPlaceHere.x ][ aPlaceHere.y ].figureItem;
			if ( mFigureToMove.figureItem.figureType == FigureType.PAWN && Math.Abs( mFigureToMove.x - aPlaceHere.x ) == 2 )
			{
				removeEnPassantPawn();
				Int32 enPassantX = ( mFigureToMove.x == 1 ? 2 : 5 );
				chessBoard[ enPassantX ][ mFigureToMove.y ].figureItem.color = mFigureToMove.figureItem.color;
				chessBoard[ enPassantX ][ mFigureToMove.y ].figureItem.figureType = FigureType.EN_PASSANT_PAWN;
			}
			else if ( enPassantFigure.figureType == FigureType.EN_PASSANT_PAWN && mFigureToMove.figureItem.figureType == FigureType.PAWN
						&& enPassantFigure.color != mFigureToMove.figureItem.color )
			{
				Int32 originalPawnX = ( aPlaceHere.x == 2 ? 3 : 4 );
				ModelItem originalPawn = chessBoard[ originalPawnX ][ aPlaceHere.y ];
				removeFigureFromWhiteOrBlack( originalPawn );
				originalPawn.figureItem = new FigureItem( Colors.NO_COLOR, FigureType.NO_FIGURE );
				fieldClicked( this, new PutFigureOnTheTableEventArg( originalPawn.x, originalPawn.y, originalPawn.index, Colors.NO_COLOR, FigureType.NO_FIGURE ) );
			}
			else
			{
				removeEnPassantPawn();
			}

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

			setLastMoveHighLights( mLastMove, false );
			mLastMove = new Move( mFigureToMove, chessBoard[ aPlaceHere.x ][ aPlaceHere.y ] );
			setLastMoveHighLights( mLastMove, true );

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

		private void setLastMoveHighLights( Move aLastMove, Boolean aSet )
		{
			if ( aLastMove == null )
			{
				return;
			}

			Colors color = ( aSet ? Colors.RED : Colors.NO_COLOR );

			setHighlight( this, new SetHighlightEventArg
			{
				index = aLastMove.itemFrom.index,
				color = color
			} );

			setHighlight( this, new SetHighlightEventArg
			{
				index = aLastMove.itemTo.index,
				color = color
			} );
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private List< Int32 > getPossibleMoves( ModelItem aFigureToMove, Boolean shouldCheckChess )
		{
			List< Int32 > pMoves = new List< Int32 >();
			switch ( aFigureToMove.figureItem.figureType )
			{
			case FigureType.MOVED_KING:
			case FigureType.KING:
				{
					KingRule kingRule		= new KingRule( chessBoard, whiteFigures, blackFigures, mPlayer1Color, aFigureToMove, shouldCheckChess );
					pMoves			= kingRule.possibleMoves();
				} break;
			case FigureType.QUEEN:
				{
					QueenRule queenRule		= new QueenRule( chessBoard, whiteFigures, blackFigures, mPlayer1Color, aFigureToMove, shouldCheckChess );
					pMoves			= queenRule.possibleMoves();
				} break;
			case FigureType.MOVED_ROOK:
			case FigureType.ROOK:
				{
					RookRule rookRule		= new RookRule( chessBoard, whiteFigures, blackFigures, mPlayer1Color, aFigureToMove, shouldCheckChess );
					pMoves			= rookRule.possibleMoves();
				} break;
			case FigureType.BISHOP:
				{
					BishopRule bishopRule	= new BishopRule( chessBoard, whiteFigures, blackFigures, mPlayer1Color, aFigureToMove, shouldCheckChess );
					pMoves			= bishopRule.possibleMoves();
				} break;
			case FigureType.KNIGHT:
				{
					KnightRule knightRule	= new KnightRule( chessBoard, whiteFigures, blackFigures, mPlayer1Color, aFigureToMove, shouldCheckChess );
					pMoves			= knightRule.possibleMoves();
				} break;
			case FigureType.PAWN:
				{
					PawnRule pawnRule		= new PawnRule( chessBoard, whiteFigures, blackFigures, mPlayer1Color, aFigureToMove, shouldCheckChess );
					pMoves			= pawnRule.possibleMoves();
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

			newKingPlace.figureItem.figureType = FigureType.MOVED_KING;
			newKingPlace.index	= aPlaceHere.index;
			newKingPlace.x		= aPlaceHere.x;
			newKingPlace.y		= aPlaceHere.y;

			fieldClicked( this, new PutFigureOnTheTableEventArg( mFigureToMove.x, mFigureToMove.y, mFigureToMove.index, Colors.NO_COLOR, FigureType.NO_FIGURE ) );
			chessBoard[ mFigureToMove.x ][ mFigureToMove.y ].figureItem = new FigureItem( Colors.NO_COLOR, FigureType.NO_FIGURE );

			fieldClicked( this, new PutFigureOnTheTableEventArg( aPlaceHere.x, aPlaceHere.y, aPlaceHere.index, mFigureToMove.figureItem.color, FigureType.MOVED_KING ) );
			chessBoard[ aPlaceHere.x ][ aPlaceHere.y ].figureItem = new FigureItem( mFigureToMove.figureItem.color, FigureType.MOVED_KING);

			// move the Rook

			fieldClicked( this, new PutFigureOnTheTableEventArg( newRookPlace.x, newRookPlace.y, newRookPlace.index, Colors.NO_COLOR, FigureType.NO_FIGURE ) );
			chessBoard[ newRookPlace.x ][ newRookPlace.y ].figureItem = new FigureItem( Colors.NO_COLOR, FigureType.NO_FIGURE );

			fieldClicked( this, new PutFigureOnTheTableEventArg( newRookPlace.x, aPlaceHere.y + moveYCoordForRookBy, aPlaceHere.index + moveYCoordForRookBy,
																							newRookPlace.figureItem.color, FigureType.MOVED_ROOK ) );
			chessBoard[ newRookPlace.x ][ aPlaceHere.y + moveYCoordForRookBy ].figureItem = new FigureItem( newRookPlace.figureItem.color, FigureType.MOVED_ROOK );

			newRookPlace.figureItem.figureType = FigureType.MOVED_ROOK;
			newRookPlace.index = aPlaceHere.index + moveYCoordForRookBy;
			newRookPlace.y = aPlaceHere.y + moveYCoordForRookBy;
			

			removeHighLights();
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private BaseAlgorithm setAlgorithm( Algorithm aAlgorithm, Colors aAlgorithmsColor )
		{
			switch ( aAlgorithm )
			{
			case Algorithm.HUMAN:							return new HumanAlgorithm( mPlayer1Color, aAlgorithmsColor );
			case Algorithm.RANDOM:							return new RandomAlgorithm( mPlayer1Color, aAlgorithmsColor );
			case Algorithm.ALPHA_BETA:						return new AlphaBetaAlgorithm( mPlayer1Color, aAlgorithmsColor );
			case Algorithm.ALPHA_BETA_RANDOM:				return new AlphaBetaAlgorithmRandom( mPlayer1Color, aAlgorithmsColor );
			case Algorithm.ALPHA_BETA_WITH_WEIGHT:			return new AlphaBetaAlgorithmWithWeight( mPlayer1Color, aAlgorithmsColor );
			case Algorithm.ALPHA_BETA_RANDOM_WITH_WEIGHT:	return new AlphaBetaAlgorithmRandomWithWeight( mPlayer1Color, aAlgorithmsColor );
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

		private void changeFigureToMoveType( FigureType newFigureType )
		{
			ModelItem figureToMove = chessBoard[ mFigureToMove.x ][ mFigureToMove.y ];
			figureToMove.figureItem.figureType = newFigureType;
			mFigureToMove.figureItem.figureType = newFigureType;
			
			List< ModelItem > myFigures = ( mFigureToMove.figureItem.color == Colors.WHITE ? whiteFigures : blackFigures );
			IEnumerable< ModelItem > myFigures2 = myFigures.Where( X => X.index == mFigureToMove.index );

			if ( myFigures2.Any() )
			{
				myFigures2.First().figureItem.figureType = newFigureType;
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public Boolean isCastling( ModelItem aFigureToMove, ModelItem aPlaceHere )
		{
			if ( aFigureToMove.figureItem.figureType != FigureType.KING )
			{
				return false;
			}

			if ( Math.Abs( aFigureToMove.y - aPlaceHere.y ) == 2 ) // do we steps 2 squares?
			{
				return true;
			}

			return false;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		void updateCastling()
		{
			if ( mFigureToMove.figureItem.figureType == FigureType.KING )
			{
				mFigureToMove.figureItem.figureType = FigureType.MOVED_KING;
				chessBoard[ mFigureToMove.x ][ mFigureToMove.y ].figureItem.figureType = FigureType.MOVED_KING;
				List< ModelItem > myFigures = ( mFigureToMove.figureItem.color == Colors.WHITE ? whiteFigures : blackFigures );
				myFigures.Where( X => X.index == mFigureToMove.index ).FirstOrDefault().figureItem.figureType = FigureType.MOVED_KING;
			}

			if ( mFigureToMove.figureItem.figureType == FigureType.ROOK )
			{
				mFigureToMove.figureItem.figureType = FigureType.MOVED_ROOK;
				chessBoard[ mFigureToMove.x ][ mFigureToMove.y ].figureItem.figureType = FigureType.MOVED_ROOK;
				List< ModelItem > myFigures = ( mFigureToMove.figureItem.color == Colors.WHITE ? whiteFigures : blackFigures );
				myFigures.Where( X => X.index == mFigureToMove.index ).FirstOrDefault().figureItem.figureType = FigureType.MOVED_ROOK;
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		void removeEnPassantPawn()
		{
			foreach ( var row in chessBoard )
			{
				foreach ( var modelItem in row )
				{
					if ( modelItem.figureItem.figureType == FigureType.EN_PASSANT_PAWN )
					{
						modelItem.figureItem.figureType = FigureType.NO_FIGURE;
						modelItem.figureItem.color		= Colors.NO_COLOR;
					}
				}
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		Boolean isGameOver( ref Colors winner )
		{
			List< ModelItem > whiteF	= new List< ModelItem >();;
			List< ModelItem > blackF	= new List< ModelItem >();;
			List< ModelItem > enemyFigures;
			List< ModelItem > myFigures;

			foreach ( var row in chessBoard )
			{
				foreach ( var modelItem in row )
				{
					if ( modelItem.figureItem.color == Colors.WHITE )
					{
						whiteF.Add( new ModelItem( modelItem.x, modelItem.y, modelItem.figureItem.color, modelItem.figureItem.figureType ) );
					}
					else if( modelItem.figureItem.color == Colors.BLACK  )
					{
						blackF.Add( new ModelItem( modelItem.x, modelItem.y, modelItem.figureItem.color, modelItem.figureItem.figureType ) );
					}
				}
			}

			if ( mCurrentColor == Colors.WHITE )
			{
				enemyFigures	= blackF;
				myFigures		= whiteF;
			}
			else
			{
				enemyFigures	= whiteF;
				myFigures		= blackF;
			}

			List< Int32 > possibleMovesOfTheItem;
			Boolean amICanNotMove = true;
			foreach ( ModelItem item in myFigures )
			{
				ModelItem modelItem = new ModelItem( item.x, item.y, item.figureItem.color, item.figureItem.figureType );
				possibleMovesOfTheItem = getPossibleMoves( item, true );
				if ( possibleMovesOfTheItem.Count != 0 )
				{
					amICanNotMove = false;
					break;
				}
			}

			Boolean isCheck = false;
			if ( amICanNotMove )
			{
				Int32 myKingPosition = myFigures.Where( X => ( X.figureItem.figureType == FigureType.KING || X.figureItem.figureType == FigureType.MOVED_KING ) ).FirstOrDefault().index;
				foreach ( ModelItem item in enemyFigures )
				{
					ModelItem modelItem = new ModelItem( item.x, item.y, item.figureItem.color, item.figureItem.figureType );
					possibleMovesOfTheItem = getPossibleMoves( item, false );
					if ( possibleMovesOfTheItem.Contains( myKingPosition ) )
					{
						isCheck = true;
						break;
					}
				}
			}

			if ( amICanNotMove && isCheck )
			{
				winner = ( mCurrentColor == Colors.WHITE ? Colors.BLACK : Colors.WHITE );
				return true;
			}

			if ( amICanNotMove && ! isCheck )
			{
				winner = Colors.NO_COLOR;
				return true;
			}

			return false;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		void refreshBlackWhiteFigures( Move aLastMove )
		{
			if ( aLastMove.itemTo.x != 0 && aLastMove.itemTo.x != 7 )
			{
				return;
			}

			List< ModelItem > itemsToCheck = ( aLastMove.itemFrom.figureItem.color == Colors.WHITE ? whiteFigures : blackFigures );

			ModelItem item = itemsToCheck.Where( X => X.index == aLastMove.itemFrom.index ).FirstOrDefault();
			if ( item.figureItem.figureType == FigureType.PAWN )
			{
				item.figureItem.figureType = aLastMove.itemFrom.figureItem.figureType;
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public event EventHandler< PutFigureOnTheTableEventArg >	fieldClicked;

		public event EventHandler< SetHighlightEventArg >			setHighlight;

		public event EventHandler< Colors >							gameOver;

		public event EventHandler< Move >							nextPlayer;

		public event EventHandler< Boolean >						setIsEnable;

		public Boolean												isGameReady { get; set; }
		public List< ModelItem >									whiteFigures { get; set; }
		public List< ModelItem >									blackFigures { get; set; }

		private List< Int32 >										possibleMoves { get; set; }
		private List< List< ModelItem > >							chessBoard { get; set; }

		//-----------------------------------------------------------------------------------------------------------------------------------------
		
		private Move												mLastMove;
		private Colors												mPlayer1Color;
		private BaseAlgorithm										mPlayer1Algorithm;
		private BaseAlgorithm										mPlayer2Algorithm;
		private Colors												mCurrentColor;

		private Boolean												mIsFirstClick;
		private Boolean												mIsBoardEnabled;

		private ModelItem											mFigureToMove;
		private Timer												mTimer;

		private FigureValues										mFigureValues;
	}
}
