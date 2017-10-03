using ChessTable.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTable.Model.Rules
{
	class BishopRule : RulesBase
	{
		
		//----------------------------------------------------------------------------------------------------------------------------------------

		public BishopRule( List< List< ModelItem > > aChessBoard, Colors aPlayer1Color, ModelItem aFigureToMove ) : base( aChessBoard, aPlayer1Color, aFigureToMove )
		{
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override List< ModelItem > possibleMoves()
		{
			mPossibleMoves = new List< ModelItem >();

			setPossibleMovesInLoop( +1, -1 ); // Lets move Down and Left;
			setPossibleMovesInLoop( +1, +1 ); // Lets move Down and Right;
			setPossibleMovesInLoop( -1, +1 ); // Lets move Up and Right;
			setPossibleMovesInLoop( -1,	-1 ); // Lets move Up and Left;

			return mPossibleMoves;
		}
		
		//----------------------------------------------------------------------------------------------------------------------------------------
	}
}
