using ChessTable.Common;
using System;
using System.Collections.Generic;

namespace ChessTable.Model.Rules
{
	public class PawnRule : RulesBase
	{
		//----------------------------------------------------------------------------------------------------------------------------------------

		public PawnRule( List< List< ModelItem > > aChessBoard, Colors aPlayer1Color, ModelItem aFigureToMove ) : base( aChessBoard, aPlayer1Color, aFigureToMove )
		{
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override List< Int32 > possibleMoves( ChessRule aChess )
		{
			mPossibleMoves						= new List< Int32 >();
			Int32 xCoord						= -1;
			Int32 yCoord						= -1;
			Boolean isValid						= false;
			Int32 isPlayer1Turn					= 1; // we adds one to X if player 1 turns
			Int32 isPlayer1Moves2				= 1; // we checks that is the pawn on line 1. If he is, he will able to step 2
			if ( mPlayer1Color == mFigureToMove.figureItem.color )
			{
				isPlayer1Turn		= -1; // we removes one from X if player 2 turns
				isPlayer1Moves2		= 6; // we checks that is the pawn on line 6. If he is, he will able to step 2
			}

			xCoord = ( mFigureToMove.x + isPlayer1Turn ); // Depends on the current player, X + 1 or X - 1
			yCoord = mFigureToMove.y;
			isValid = isValidField( xCoord, yCoord );
			if ( isValid && mChessBoard[ xCoord ][ yCoord ].figureItem.figureType == FigureType.NO_FIGURE ) // Can we move forward?
			{
				mPossibleMoves.Add( ( 8 * xCoord ) + yCoord );

				if ( mFigureToMove.x == isPlayer1Moves2 && mChessBoard[ xCoord + isPlayer1Turn ][ yCoord ].figureItem.figureType == FigureType.NO_FIGURE ) // Can we make 2 steps forward?
				{
					mPossibleMoves.Add( ( 8 * ( xCoord + isPlayer1Turn ) ) + yCoord );
				}
			}

			//------

			yCoord		= ( mFigureToMove.y - 1 );
			isValid		= isValidField( xCoord, yCoord );
			if ( isValid && ( mChessBoard[ xCoord ][ yCoord ].figureItem.figureType != FigureType.NO_FIGURE ) ) // Can we hit someone on the left?
			{
				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				mPossibleMoves.Add( ( 8 * xCoord ) + yCoord );
			}

			//------

			yCoord		= ( mFigureToMove.y + 1 );
			isValid		= isValidField( xCoord, yCoord );
			if ( isValid && ( mChessBoard[ xCoord ][ yCoord ].figureItem.figureType != FigureType.NO_FIGURE ) ) // Can we hit someone on the right?
			{
				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				mPossibleMoves.Add( ( 8 * xCoord ) + yCoord );
			}

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
