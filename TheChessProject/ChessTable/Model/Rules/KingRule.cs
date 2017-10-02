using ChessTable.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTable.Model.Rules
{
	class KingRule : RulesBase
	{
		
		//----------------------------------------------------------------------------------------------------------------------------------------

		public KingRule( List< List< ModelItem > > aChessBoard, Colors aPlayer1Color, ModelItem aFigureToMove ) : base( aChessBoard, aPlayer1Color, aFigureToMove )
		{
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override List< ModelItem > setPossibleMoves()
		{
			List< ModelItem > possibleMoves		= new List< ModelItem >();

			Int32 xCoord						= mFigureToMove.x + 1; // Lets move Down;
			Int32 yCoord						= mFigureToMove.y - 1;
			if ( isValidField( xCoord, yCoord ) )
			{
				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				possibleMoves.Add( new ModelItem( xCoord, yCoord, figureItem.color, figureItem.figureType ) );
			}

			xCoord								= mFigureToMove.x + 1; // Lets move Down;
			yCoord								= mFigureToMove.y;
			if ( isValidField( xCoord, yCoord ) )
			{
				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				possibleMoves.Add( new ModelItem( xCoord, yCoord, figureItem.color, figureItem.figureType ) );
			}

			xCoord								= mFigureToMove.x + 1; // Lets move Down;
			yCoord								= mFigureToMove.y + 1;
			if ( isValidField( xCoord, yCoord ) )
			{
				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				possibleMoves.Add( new ModelItem( xCoord, yCoord, figureItem.color, figureItem.figureType ) );
			}

			xCoord								= mFigureToMove.x; // Lets move Down;
			yCoord								= mFigureToMove.y - 1;
			if ( isValidField( xCoord, yCoord ) )
			{
				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				possibleMoves.Add( new ModelItem( xCoord, yCoord, figureItem.color, figureItem.figureType ) );
			}

			xCoord								= mFigureToMove.x; // Lets move Down;
			yCoord								= mFigureToMove.y + 1;
			if ( isValidField( xCoord, yCoord ) )
			{
				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				possibleMoves.Add( new ModelItem( xCoord, yCoord, figureItem.color, figureItem.figureType ) );
			}

			xCoord								= mFigureToMove.x - 1; // Lets move Down;
			yCoord								= mFigureToMove.y - 1;
			if ( isValidField( xCoord, yCoord ) )
			{
				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				possibleMoves.Add( new ModelItem( xCoord, yCoord, figureItem.color, figureItem.figureType ) );
			}

			xCoord								= mFigureToMove.x - 1; // Lets move Down;
			yCoord								= mFigureToMove.y;
			if ( isValidField( xCoord, yCoord ) )
			{
				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				possibleMoves.Add( new ModelItem( xCoord, yCoord, figureItem.color, figureItem.figureType ) );
			}

			xCoord								= mFigureToMove.x - 1; // Lets move Down;
			yCoord								= mFigureToMove.y + 1;
			if ( isValidField( xCoord, yCoord ) )
			{
				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				possibleMoves.Add( new ModelItem( xCoord, yCoord, figureItem.color, figureItem.figureType ) );
			}

			return possibleMoves;
		}
		
		//----------------------------------------------------------------------------------------------------------------------------------------
	}
}
