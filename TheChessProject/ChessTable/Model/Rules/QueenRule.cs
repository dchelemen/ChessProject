using ChessTable.Common;
using System;
using System.Collections.Generic;

namespace ChessTable.Model.Rules
{
	class QueenRule : RulesBase
	{
		
		//----------------------------------------------------------------------------------------------------------------------------------------

		public QueenRule( List< List< ModelItem > > aChessBoard, Colors aPlayer1Color, ModelItem aFigureToMove ) : base( aChessBoard, aPlayer1Color, aFigureToMove )
		{
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override List< Int32 > possibleMoves()
		{
			mPossibleMoves = new List< Int32 >();

			setPossibleMovesInLoop( +1, -1 ); // Lets move Down and Left;
			setPossibleMovesInLoop( +1, +0 ); // Lets move Down;
			setPossibleMovesInLoop( +1, +1 ); // Lets move Down and Right;
			setPossibleMovesInLoop( +0, +1 ); // Lets move Right;
			setPossibleMovesInLoop( -1, +1 ); // Lets move Up and Right;
			setPossibleMovesInLoop( -1, +0 ); // Lets move Up;
			setPossibleMovesInLoop( -1, -1 ); // Lets move Up and Left;
			setPossibleMovesInLoop( +0, -1 ); // Lets move Left;

			return mPossibleMoves;
		}
		
		//----------------------------------------------------------------------------------------------------------------------------------------
	}
}
