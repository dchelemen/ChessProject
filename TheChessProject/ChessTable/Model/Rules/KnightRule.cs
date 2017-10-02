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

			// 2 Left, 1 Up;
			setOnePossibleMove( ref possibleMoves, -1, -2 );

			// 2 Left, 1 Down;
			setOnePossibleMove( ref possibleMoves, 1, -2 );

			// 2 Down, 1 Left;
			setOnePossibleMove( ref possibleMoves, 2, -1 );

			// 2 Down, 1 Right;
			setOnePossibleMove( ref possibleMoves, 2, 1 );

			// 2 Right, 1 Down;
			setOnePossibleMove( ref possibleMoves, 1, 2 );

			// 2 Right, 1 Up;
			setOnePossibleMove( ref possibleMoves, -1, 2 );

			// 2 Up, 1 Right;
			setOnePossibleMove( ref possibleMoves, -2, 1 );

			// 2 Up, 1 Left;
			setOnePossibleMove( ref possibleMoves, -2, -1 );

			return possibleMoves;
		}
		
		//----------------------------------------------------------------------------------------------------------------------------------------
	}
}
