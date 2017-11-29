using ChessTable.Common;
using ChessTable.Model.Rules;
using System;
using System.Collections.Generic;

namespace ChessTable.Model
{
	public abstract class RulesBase
	{
		//----------------------------------------------------------------------------------------------------------------------------------------

		public RulesBase( List< List< ModelItem > > aChessBoard, Colors aPlayer1Color, ModelItem aFigureToMove )
		{
			mChessBoard		= aChessBoard;
			mPlayer1Color	= aPlayer1Color;
			mFigureToMove	= aFigureToMove;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public abstract List< Int32 > possibleMoves( ChessRule aChess );

		//----------------------------------------------------------------------------------------------------------------------------------------

		protected void setPossibleMovesInLoop( Int32 aAddX, Int32 aAddY )
		{
			Int32 xCoord	= mFigureToMove.x + aAddX;
			Int32 yCoord	= mFigureToMove.y + aAddY;
			while ( isValidField( xCoord, yCoord ) )
			{
				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				mPossibleMoves.Add( ( 8 * xCoord ) + yCoord );

				if ( figureItem.figureType != FigureType.NO_FIGURE ) // when enemies found, break!
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

		protected List< List < ModelItem > >		mChessBoard { get; set; }
		protected List< Int32 >						mPossibleMoves { get; set; }

		//----------------------------------------------------------------------------------------------------------------------------------------

		protected Colors							mPlayer1Color;
		protected ModelItem							mFigureToMove;
	}
}
