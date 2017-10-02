using ChessTable.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTable.Model.Rules
{
	class KnightRule : RulesBase
	{
		
		//----------------------------------------------------------------------------------------------------------------------------------------

		public KnightRule( List< List< ModelItem > > aChessBoard, Colors aPlayer1Color, ModelItem aFigureToMove ) : base( aChessBoard, aPlayer1Color, aFigureToMove )
		{
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override List< ModelItem > setPossibleMoves()
		{
			List< ModelItem > possibleMoves		= new List< ModelItem >();

			Int32 xCoord						= mFigureToMove.x - 1; // 2 Left, 1 Up;
			Int32 yCoord						= mFigureToMove.y - 2;
			if ( isValidField( xCoord, yCoord ) )
			{
				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				possibleMoves.Add( new ModelItem( xCoord, yCoord, figureItem.color, figureItem.figureType ) );
			}

			xCoord								= mFigureToMove.x + 1; // 2 Left, 1 Down;
			yCoord								= mFigureToMove.y - 2;
			if ( isValidField( xCoord, yCoord ) )
			{
				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				possibleMoves.Add( new ModelItem( xCoord, yCoord, figureItem.color, figureItem.figureType ) );
			}

			xCoord								= mFigureToMove.x + 2; // 2 Down, 1 Left;
			yCoord								= mFigureToMove.y - 1;
			if ( isValidField( xCoord, yCoord ) )
			{
				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				possibleMoves.Add( new ModelItem( xCoord, yCoord, figureItem.color, figureItem.figureType ) );
			}

			xCoord								= mFigureToMove.x + 2; // 2 Down, 1 Right;
			yCoord								= mFigureToMove.y + 1;
			if ( isValidField( xCoord, yCoord ) )
			{
				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				possibleMoves.Add( new ModelItem( xCoord, yCoord, figureItem.color, figureItem.figureType ) );
			}

			xCoord								= mFigureToMove.x + 1; // 2 Right, 1 Down;
			yCoord								= mFigureToMove.y + 2;
			if ( isValidField( xCoord, yCoord ) )
			{
				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				possibleMoves.Add( new ModelItem( xCoord, yCoord, figureItem.color, figureItem.figureType ) );
			}

			xCoord								= mFigureToMove.x - 1; // 2 Right, 1 Up;
			yCoord								= mFigureToMove.y + 2;
			if ( isValidField( xCoord, yCoord ) )
			{
				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				possibleMoves.Add( new ModelItem( xCoord, yCoord, figureItem.color, figureItem.figureType ) );
			}

			xCoord								= mFigureToMove.x - 2; // 2 Up, 1 Right;
			yCoord								= mFigureToMove.y + 1;
			if ( isValidField( xCoord, yCoord ) )
			{
				FigureItem figureItem = mChessBoard[ xCoord ][ yCoord ].figureItem;
				possibleMoves.Add( new ModelItem( xCoord, yCoord, figureItem.color, figureItem.figureType ) );
			}

			xCoord								= mFigureToMove.x - 2; // 2 Up, 1 Left;
			yCoord								= mFigureToMove.y - 1;
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
