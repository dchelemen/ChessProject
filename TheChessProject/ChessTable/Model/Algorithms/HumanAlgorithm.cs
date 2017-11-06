using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTable.Model.Algorithms
{
	public class HumanAlgorithm : BaseAlgorithm
	{
		
		public HumanAlgorithm() : base()
		{
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public override void setTree( List< ModelItem > aPlayersFigures, List< ModelItem > enemyFigures)
		{

		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public override void refreshTree( Move aLastMove )
		{

		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public override Boolean isActive()
		{
			return false;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override Move nextMove()
		{
			return new Move();
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

	}
}
