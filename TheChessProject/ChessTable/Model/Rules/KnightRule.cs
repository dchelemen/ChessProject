using ChessTable.Common;
using System;
using System.Collections.Generic;

namespace ChessTable.Model.Rules
{
	class KnightRule : RulesBase
	{
		
		//----------------------------------------------------------------------------------------------------------------------------------------

		public KnightRule( List< List< ModelItem > > aChessBoard, Colors aPlayer1Color, ModelItem aFigureToMove ) : base( aChessBoard, aPlayer1Color, aFigureToMove )
		{
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override List< Int32 > possibleMoves( ChessRule aChess )
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

			if ( aChess.isChess )
			{
				for ( Int32 i = ( mPossibleMoves.Count - 1 ); i >= 0; i-- )
				{
					if ( ! aChess.positionsToBreakChess.Contains( mPossibleMoves[ i ] ) && mPossibleMoves[ i ] != aChess.chessGiverPosition )
					{
						mPossibleMoves.RemoveAt( i );
					}
				}
			}
			return mPossibleMoves;
		}
		
		//----------------------------------------------------------------------------------------------------------------------------------------
	}
}
