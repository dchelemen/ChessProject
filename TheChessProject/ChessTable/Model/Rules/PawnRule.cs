using ChessTable.Common;
using System;
using System.Collections.Generic;

namespace ChessTable.Model.Rules
{
	public class PawnRule : RulesBase
	{
		//----------------------------------------------------------------------------------------------------------------------------------------

		public PawnRule( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures, Colors aPlayer1Color, ModelItem aFigureToMove, Boolean aIsCheckChess )
			: base( aChessBoard, aWhiteFigures, aBlackFigures, aPlayer1Color, aFigureToMove, aIsCheckChess )
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
			
			Boolean canWeMoveForward = false;
			xCoord = ( mFigureToMove.x + isPlayer1Turn ); // Depends on the current player, X + 1 or X - 1
			yCoord = mFigureToMove.y;
			isValid = isValidField( xCoord, yCoord );
			if ( isValid && mChessBoard[ xCoord ][ yCoord ].figureItem.figureType == FigureType.NO_FIGURE ) // Can we move forward?
			{
				canWeMoveForward = true;
				if( ! mIsCheckChess || ! isChess( xCoord, yCoord ) )
				{
					mPossibleMoves.Add( ( 8 * xCoord ) + yCoord );
				}
			}

			//------

			xCoord += isPlayer1Turn; // Depends on the current player, X + 1 or X - 1
			isValid = isValidField( xCoord, yCoord );
			if ( canWeMoveForward && mFigureToMove.x == isPlayer1Moves2 && mChessBoard[ xCoord ][ yCoord ].figureItem.figureType == FigureType.NO_FIGURE ) // Can we make 2 steps forward?
			{
				if( ! mIsCheckChess || ! isChess( xCoord, yCoord ) )
				{
					mPossibleMoves.Add( ( 8 * ( xCoord ) ) + yCoord );
				}
			}
			xCoord -= isPlayer1Turn;

			//------

			yCoord		= ( mFigureToMove.y - 1 );
			isValid		= isValidField( xCoord, yCoord );
			if ( isValid && ( mChessBoard[ xCoord ][ yCoord ].figureItem.figureType != FigureType.NO_FIGURE ) ) // Can we hit someone on the left?
			{
				if( ! mIsCheckChess || ! isChess( xCoord, yCoord ) )
				{
					FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
					mPossibleMoves.Add( ( 8 * xCoord ) + yCoord );
				}
			}

			//------

			yCoord		= ( mFigureToMove.y + 1 );
			isValid		= isValidField( xCoord, yCoord );
			if ( isValid && ( mChessBoard[ xCoord ][ yCoord ].figureItem.figureType != FigureType.NO_FIGURE ) ) // Can we hit someone on the right?
			{
				if( ! mIsCheckChess || ! isChess( xCoord, yCoord ) )
				{
					FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
					mPossibleMoves.Add( ( 8 * xCoord ) + yCoord );
				}
			}

			return mPossibleMoves;
		}
		
		//----------------------------------------------------------------------------------------------------------------------------------------
	}
}
