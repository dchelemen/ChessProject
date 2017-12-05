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

		public override void setTree( List< List< ModelItem > > aChessBoard, List< ModelItem > aPlayersFigures, List< ModelItem > enemyFigures )
		{

		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public override void refreshTree( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures, Move aLastMove )
		{

		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override void move( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures )
		{
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

	}
}
