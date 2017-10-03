using ChessTable.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTable.Model.Rules
{
	class RookRule : RulesBase
	{
		
		//----------------------------------------------------------------------------------------------------------------------------------------

		public RookRule( List< List< ModelItem > > aChessBoard, Colors aPlayer1Color, ModelItem aFigureToMove ) : base( aChessBoard, aPlayer1Color, aFigureToMove )
		{
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override List< ModelItem > possibleMoves()
		{
			List< ModelItem > mPossibleMoves		= new List< ModelItem >();

			// Lets move Down;
			setPossibleMovesInLoop( 1, 0 );

			// Lets move Up;
			setPossibleMovesInLoop( -1, 0 );

			// Lets move Right;
			setPossibleMovesInLoop( 0, 1 );

			// Lets move Left;
			setPossibleMovesInLoop( 0, -1 );

			return mPossibleMoves;
		}
		
		//----------------------------------------------------------------------------------------------------------------------------------------
	}
}
