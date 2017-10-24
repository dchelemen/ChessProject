using ChessTable.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public abstract List< ModelItem > possibleMoves();

		//----------------------------------------------------------------------------------------------------------------------------------------

		protected void setPossibleMovesInLoop( Int32 aAddX, Int32 aAddY )
		{
			Int32 xCoord	= mFigureToMove.x + aAddX;
			Int32 yCoord	= mFigureToMove.y + aAddY;
			while ( isValidField( xCoord, yCoord ) )
			{
				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				mPossibleMoves.Add( new ModelItem( xCoord, yCoord, figureItem.color, figureItem.figureType ) );

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
				mPossibleMoves.Add( new ModelItem( xCoord, yCoord, figureItem.color, figureItem.figureType ) );
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
		protected List< ModelItem >					mPossibleMoves { get; set; }

		//----------------------------------------------------------------------------------------------------------------------------------------

		protected Colors							mPlayer1Color;
		protected ModelItem							mFigureToMove;
	}
}
