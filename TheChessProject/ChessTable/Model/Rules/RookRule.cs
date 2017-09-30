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

		public override List< ModelItem > setPossibleMoves()
		{
			List< ModelItem > possibleMoves		= new List< ModelItem >();

			Int32 xCoord						= mFigureToMove.x + 1; // Lets move Down;
			Int32 yCoord						= mFigureToMove.y;

			while ( isValidField( xCoord, yCoord ) )
			{
				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				possibleMoves.Add( new ModelItem( xCoord, yCoord, figureItem.color, figureItem.figureType ) );

				if ( figureItem.figureType != FigureType.NO_FIGURE ) // when enemies found, break!
				{
					break;
				}

				xCoord += 1;
			}

			xCoord								= mFigureToMove.x - 1; // Lets move Up;
			yCoord								= mFigureToMove.y;

			while ( isValidField( xCoord, yCoord ) )
			{
				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				possibleMoves.Add( new ModelItem( xCoord, yCoord, figureItem.color, figureItem.figureType ) );

				if ( figureItem.figureType != FigureType.NO_FIGURE ) // when enemies found, break!
				{
					break;
				}

				xCoord -= 1;
			}

			xCoord								= mFigureToMove.x; // Lets move Right;
			yCoord								= mFigureToMove.y + 1;

			while ( isValidField( xCoord, yCoord ) )
			{
				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				possibleMoves.Add( new ModelItem( xCoord, yCoord, figureItem.color, figureItem.figureType ) );

				if ( figureItem.figureType != FigureType.NO_FIGURE ) // when enemies found, break!
				{
					break;
				}

				yCoord += 1;
			}

			xCoord								= mFigureToMove.x; // Lets move Left;
			yCoord								= mFigureToMove.y - 1;

			while ( isValidField( xCoord, yCoord ) )
			{
				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				possibleMoves.Add( new ModelItem( xCoord, yCoord, figureItem.color, figureItem.figureType ) );

				if ( figureItem.figureType != FigureType.NO_FIGURE ) // when enemies found, break!
				{
					break;
				}

				yCoord -= 1;
			}

			return possibleMoves;
		}
		
		//----------------------------------------------------------------------------------------------------------------------------------------

		protected override Boolean isValidField( Int32 aX, Int32 aY )
		{
			if ( aX < 0 || aX > 7 || aY < 0 || aY > 7 )
			{
				return false;
			}

			if ( mChessBoard[ aX ][ aY ].figureItem.color == mFigureToMove.figureItem.color )
			{
				return false;
			}

			return true;
		}
	}
}
