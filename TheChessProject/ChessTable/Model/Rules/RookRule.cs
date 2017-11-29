using ChessTable.Common;
using System;
using System.Collections.Generic;

namespace ChessTable.Model.Rules
{
	class RookRule : RulesBase
	{
		
		//----------------------------------------------------------------------------------------------------------------------------------------

		public RookRule( List< List< ModelItem > > aChessBoard, Colors aPlayer1Color, ModelItem aFigureToMove ) : base( aChessBoard, aPlayer1Color, aFigureToMove )
		{
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override List< Int32 > possibleMoves( ChessRule aChess )
		{
			mPossibleMoves = new List< Int32 >();

			setPossibleMovesInLoop( +1, +0 ); // Lets move Down;
			setPossibleMovesInLoop( -1, +0 ); // Lets move Up;
			setPossibleMovesInLoop( +0, +1 ); // Lets move Right;
			setPossibleMovesInLoop( +0, -1 ); // Lets move Left;

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
