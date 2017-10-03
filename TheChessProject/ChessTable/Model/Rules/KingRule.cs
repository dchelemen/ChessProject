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

		public override List< ModelItem > possibleMoves()
		{
			mPossibleMoves		= new List< ModelItem >();

			// Lets move Down and Left;
			setOnePossibleMove( 1, -1 );

			// Lets move Down;
			setOnePossibleMove( 1, 0 );

			// Lets move Down and Right;
			setOnePossibleMove( 1, 1 );

			// Lets move Right;
			setOnePossibleMove( 0, 1 );

			// Lets move Up and Right;
			setOnePossibleMove( -1, 1 );

			// Lets move Up;
			setOnePossibleMove( -1, 0 );

			// Lets move Up and Left;
			setOnePossibleMove( -1, -1 );

			// Lets move Left;
			setOnePossibleMove( 0, -1 );

			return mPossibleMoves;
		}
		
		//----------------------------------------------------------------------------------------------------------------------------------------
	}
}
