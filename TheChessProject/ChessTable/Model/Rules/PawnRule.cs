using ChessTable.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTable.Model.Rules
{
	public class PawnRule : RulesBase
	{
		//----------------------------------------------------------------------------------------------------------------------------------------

		public PawnRule( List< List< ModelItem > > aChessBoard, Colors aPlayer1Color, ModelItem aFigureToMove ) : base( aChessBoard, aPlayer1Color, aFigureToMove )
		{
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override List< ModelItem > setPossibleMoves()
		{
			List< ModelItem > possibleMoves		= new List< ModelItem >();
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
				possibleMoves.Add( new ModelItem( xCoord, yCoord, Colors.NO_COLOR, FigureType.NO_FIGURE ) );

				if ( mFigureToMove.x == isPlayer1Moves2 && mChessBoard[ xCoord + isPlayer1Turn ][ yCoord ].figureItem.figureType == FigureType.NO_FIGURE ) // Can we make 2 steps forward?
				{
					possibleMoves.Add( new ModelItem( xCoord + isPlayer1Turn, yCoord, Colors.NO_COLOR, FigureType.NO_FIGURE ) );
				}
			}

			//------

			yCoord		= ( mFigureToMove.y - 1 );
			isValid		= isValidField( xCoord, yCoord );
			if ( isValid && ( mChessBoard[ xCoord ][ yCoord ].figureItem.figureType != FigureType.NO_FIGURE ) ) // Can we hit someone on the left?
			{
				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				possibleMoves.Add( new ModelItem( xCoord, yCoord, figureItem.color, figureItem.figureType ) );
			}

			//------

			yCoord		= ( mFigureToMove.y + 1 );
			isValid		= isValidField( xCoord, yCoord );
			if ( isValid && ( mChessBoard[ xCoord ][ yCoord ].figureItem.figureType != FigureType.NO_FIGURE ) ) // Can we hit someone on the right?
			{
				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				possibleMoves.Add( new ModelItem( xCoord, yCoord, figureItem.color, figureItem.figureType ) );
			}

			return possibleMoves;
		}
		
		//----------------------------------------------------------------------------------------------------------------------------------------
	}
}
