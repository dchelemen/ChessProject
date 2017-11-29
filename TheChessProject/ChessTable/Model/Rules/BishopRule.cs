using ChessTable.Common;
using System;
using System.Collections.Generic;

namespace ChessTable.Model.Rules
{
	class BishopRule : RulesBase
	{
		
		//----------------------------------------------------------------------------------------------------------------------------------------

		public BishopRule( List< List< ModelItem > > aChessBoard, Colors aPlayer1Color, ModelItem aFigureToMove ) : base( aChessBoard, aPlayer1Color, aFigureToMove )
		{
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override List< Int32 > possibleMoves( ChessRule aChess )
		{
			mPossibleMoves = new List< Int32 >();

			setPossibleMovesInLoop( +1, -1 ); // Lets move Down and Left;
			setPossibleMovesInLoop( +1, +1 ); // Lets move Down and Right;
			setPossibleMovesInLoop( -1, +1 ); // Lets move Up and Right;
			setPossibleMovesInLoop( -1,	-1 ); // Lets move Up and Left;

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
