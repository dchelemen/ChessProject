using ChessTable.Common;
using ChessTable.Model.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessTable.Model
{
	public abstract class RulesBase
	{
		//----------------------------------------------------------------------------------------------------------------------------------------

		public RulesBase( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures, Colors aPlayer1Color, ModelItem aFigureToMove, Boolean aIsCheckChess )
		{
			mChessBoard		= aChessBoard;
			mWhiteFigures	= aWhiteFigures;
			mBlackFigures	= aBlackFigures;
			mPlayer1Color	= aPlayer1Color;
			mFigureToMove	= aFigureToMove;
			mIsCheckChess	= aIsCheckChess;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public abstract List< Int32 > possibleMoves();

		//----------------------------------------------------------------------------------------------------------------------------------------

		protected void setPossibleMovesInLoop( Int32 aAddX, Int32 aAddY )
		{
			Int32 xCoord	= mFigureToMove.x + aAddX;
			Int32 yCoord	= mFigureToMove.y + aAddY;
			while ( isValidField( xCoord, yCoord ) )
			{
				if( mIsCheckChess && isChess( xCoord, yCoord ) )
				{
					xCoord += aAddX;
					yCoord += aAddY;
					continue;
				}

				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				mPossibleMoves.Add( ( 8 * xCoord ) + yCoord );
				
				List< ModelItem > enemyFigures = ( mFigureToMove.figureItem.color == Colors.WHITE ? mBlackFigures : mWhiteFigures );
				if ( figureItem.figureType != FigureType.NO_FIGURE && enemyFigures.Contains( mChessBoard[ xCoord ][ yCoord ] ) ) // when enemies found, break!
				{
					break;
				}

				xCoord += aAddX;
				yCoord += aAddY;
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		protected virtual void setOnePossibleMove( Int32 aAddX, Int32 aAddY )
		{
			Int32 xCoord	= mFigureToMove.x + aAddX;
			Int32 yCoord	= mFigureToMove.y + aAddY;
			if ( isValidField( xCoord, yCoord ) )
			{
				if( mIsCheckChess && isChess( xCoord, yCoord ) )
				{
					return;
				}

				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				mPossibleMoves.Add( ( 8 * xCoord ) + yCoord );
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		protected Boolean isValidField( Int32 aX, Int32 aY )
		{
			if ( aX < 0 || aX > 7 || aY < 0 || aY > 7 )
			{
				return false;
			}

			if ( mChessBoard[ aX ][ aY ].figureItem.color == mFigureToMove.figureItem.color )
			{
				return false;
			}

			return true;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		protected Boolean isChess( Int32 aX, Int32 aY )
		{
			Boolean noChess									= false;

			List< ModelItem > enemyFigures;
			List< ModelItem > myFigures;

			if ( mFigureToMove.figureItem.color == Colors.WHITE )
			{
				myFigures		= mWhiteFigures;
				enemyFigures	= mBlackFigures;
			}
			else
			{
				myFigures		= mBlackFigures;
				enemyFigures	= mWhiteFigures;
			}

			FigureItem targetItem							= new FigureItem( mChessBoard[ aX ][ aY ].figureItem.color, mChessBoard[ aX ][ aY ].figureItem.figureType );
			mChessBoard[ aX ][ aY ].figureItem.color		= mFigureToMove.figureItem.color;
			mChessBoard[ aX ][ aY ].figureItem.figureType	= mFigureToMove.figureItem.figureType;
			mChessBoard[ mFigureToMove.x ][ mFigureToMove.y ].figureItem.color					= Colors.NO_COLOR;
			mChessBoard[ mFigureToMove.x ][ mFigureToMove.y ].figureItem.figureType				= FigureType.NO_FIGURE;
			ModelItem myFigure = myFigures.Where( X => X.index == mFigureToMove.index ).FirstOrDefault();
			myFigure.x = aX;
			myFigure.y = aY;
			myFigure.index = ( aX * 8 ) + aY;
			
			Int32 MyKingPosition = myFigures.Where( X => X.figureItem.figureType == FigureType.KING ).FirstOrDefault().index;
			foreach ( ModelItem figure in enemyFigures )
			{
				if ( figure.x == aX && figure.y == aY )
				{
					continue;
				}

				List< Int32 > possibleMoves = getPossibleMoves( figure );
				if ( possibleMoves.Contains( MyKingPosition ) )
				{
					noChess = true;
					break;
				}
			}

			mChessBoard[ mFigureToMove.x ][ mFigureToMove.y ].figureItem.color					= mChessBoard[ aX ][ aY ].figureItem.color;
			mChessBoard[ mFigureToMove.x ][ mFigureToMove.y ].figureItem.figureType				= mChessBoard[ aX ][ aY ].figureItem.figureType;
			mChessBoard[ aX ][ aY ].figureItem.color		= targetItem.color;
			mChessBoard[ aX ][ aY ].figureItem.figureType	= targetItem.figureType;
			myFigure.x = mFigureToMove.x;
			myFigure.y = mFigureToMove.y;
			myFigure.index = mFigureToMove.index;

			return noChess;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private List< Int32 > getPossibleMoves( ModelItem aFigureToMove )
		{
			List< Int32 > pMoves = new List< Int32 >();
			switch ( aFigureToMove.figureItem.figureType )
			{
			case FigureType.QUEEN:
				{
					QueenRule queenRule		= new QueenRule( mChessBoard, mWhiteFigures, mBlackFigures, mPlayer1Color, aFigureToMove, false );
					pMoves			= queenRule.possibleMoves();
				} break;
			case FigureType.ROOK:
				{
					RookRule rookRule		= new RookRule( mChessBoard, mWhiteFigures, mBlackFigures, mPlayer1Color, aFigureToMove, false );
					pMoves			= rookRule.possibleMoves();
				} break;
			case FigureType.BISHOP:
				{
					BishopRule bishopRule	= new BishopRule( mChessBoard, mWhiteFigures, mBlackFigures, mPlayer1Color, aFigureToMove, false );
					pMoves			= bishopRule.possibleMoves();
				} break;
			case FigureType.KNIGHT:
				{
					KnightRule knightRule	= new KnightRule( mChessBoard, mWhiteFigures, mBlackFigures, mPlayer1Color, aFigureToMove, false );
					pMoves			= knightRule.possibleMoves();
				} break;
			case FigureType.PAWN:
				{
					PawnRule pawnRule		= new PawnRule( mChessBoard, mWhiteFigures, mBlackFigures, mPlayer1Color, aFigureToMove, false );
					pMoves			= pawnRule.possibleMoves();
				} break;
			case FigureType.NO_FIGURE:		break;
			}

			return pMoves;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		protected List< List < ModelItem > >		mChessBoard { get; set; }
		protected List< ModelItem >					mWhiteFigures { get; set; }
		protected List< ModelItem >					mBlackFigures { get; set; }
		protected List< Int32 >						mPossibleMoves { get; set; }

		//----------------------------------------------------------------------------------------------------------------------------------------

		protected Colors							mPlayer1Color;
		protected ModelItem							mFigureToMove;
		protected Int32								mKingIndex;
		protected Boolean							mIsCheckChess;
	}
}
