using ChessTable.Common;
using ChessTable.Model.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTable.Model.Algorithms
{
	public class HumanAlgorithm : BaseAlgorithm
	{
		
		public HumanAlgorithm( Colors aPlayer1Color, Colors aMyColor ) : base( aPlayer1Color, aMyColor )
		{
			isActive = false;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public override void setTree( List< List< ModelItem > > aChessBoard, List< ModelItem > aPlayersFigures, List< ModelItem > enemyFigures, CastlingRule aCastlingRule, ChessRule aChess )
		{

		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public override void refreshTree( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures, CastlingRule aCastlingRule, Move aLastMove, ChessRule aChess )
		{

		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override Move nextMove( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures, CastlingRule aCastlingRule )
		{
			return new Move();
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

	}
}
