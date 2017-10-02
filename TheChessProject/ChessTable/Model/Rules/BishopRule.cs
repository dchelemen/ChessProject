using ChessTable.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTable.Model.Rules
{
	class BishopRule : RulesBase
	{
		
		//----------------------------------------------------------------------------------------------------------------------------------------

		public BishopRule( List< List< ModelItem > > aChessBoard, Colors aPlayer1Color, ModelItem aFigureToMove ) : base( aChessBoard, aPlayer1Color, aFigureToMove )
		{
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override List< ModelItem > setPossibleMoves()
		{
			List< ModelItem > possibleMoves		= new List< ModelItem >();
			
			// Lets move Down and Left;
			setPossibleMovesInLoop( ref possibleMoves, 1, -1 );

			// Lets move Down and Right;
			setPossibleMovesInLoop( ref possibleMoves, 1, 1 );

			// Lets move Up and Right;
			setPossibleMovesInLoop( ref possibleMoves, -1, 1 );

			// Lets move Up and Left;
			setPossibleMovesInLoop( ref possibleMoves, -1, -1 );

			return possibleMoves;
		}
		
		//----------------------------------------------------------------------------------------------------------------------------------------
	}
}
