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

			// Lets move Down and Left;
			setOnePossibleMove( ref possibleMoves, 1, -1 );

			// Lets move Down;
			setOnePossibleMove( ref possibleMoves, 1, 0 );

			// Lets move Down and Right;
			setOnePossibleMove( ref possibleMoves, 1, 1 );

			// Lets move Right;
			setOnePossibleMove( ref possibleMoves, 0, 1 );

			// Lets move Up and Right;
			setOnePossibleMove( ref possibleMoves, -1, 1 );

			// Lets move Up;
			setOnePossibleMove( ref possibleMoves, -1, 0 );

			// Lets move Up and Left;
			setOnePossibleMove( ref possibleMoves, -1, -1 );

			// Lets move Left;
			setOnePossibleMove( ref possibleMoves, 0, -1 );

			return possibleMoves;
		}
		
		//----------------------------------------------------------------------------------------------------------------------------------------
	}
}
