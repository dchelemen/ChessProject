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

		public abstract List< ModelItem > setPossibleMoves();

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

		//----------------------------------------------------------------------------------------------------------------------------------------

		protected Colors							mPlayer1Color;
		protected ModelItem							mFigureToMove;
	}
}
