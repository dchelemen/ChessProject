using ChessTable.Common;
using System;
using System.Collections.Generic;

namespace ChessTable.Model.Rules
{
	class KnightRule : RulesBase
	{
		
		//----------------------------------------------------------------------------------------------------------------------------------------

		public KnightRule( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures, Colors aPlayer1Color, ModelItem aFigureToMove, Boolean aIsCheckChess )
			: base( aChessBoard, aWhiteFigures, aBlackFigures, aPlayer1Color, aFigureToMove, aIsCheckChess )
		{
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override List< Int32 > possibleMoves()
		{
			mPossibleMoves = new List< Int32 >();

			setOnePossibleMove( -1, -2 ); // 2 Left, 1 Up;
			setOnePossibleMove( +1, -2 ); // 2 Left, 1 Down;
			setOnePossibleMove( +2, -1 ); // 2 Down, 1 Left;
			setOnePossibleMove( +2, +1 ); // 2 Down, 1 Right;
			setOnePossibleMove( +1, +2 ); // 2 Right, 1 Down;
			setOnePossibleMove( -1, +2 ); // 2 Right, 1 Up;
			setOnePossibleMove( -2, +1 ); // 2 Up, 1 Right;
			setOnePossibleMove( -2, -1 ); // 2 Up, 1 Left;

			return mPossibleMoves;
		}
		
		//----------------------------------------------------------------------------------------------------------------------------------------
	}
}
