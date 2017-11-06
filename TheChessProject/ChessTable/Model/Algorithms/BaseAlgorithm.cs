using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTable.Model.Algorithms
{
	public abstract class BaseAlgorithm
	{
		public BaseAlgorithm()
		{
			mTree = null;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public abstract void setTree( List< ModelItem > aPlayersFigures, List< ModelItem > enemyFigures);

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public abstract Boolean isActive();

		//----------------------------------------------------------------------------------------------------------------------------------------

		public abstract void refreshTree( Move aLastMove );

		//----------------------------------------------------------------------------------------------------------------------------------------

		public abstract Move nextMove();

		//----------------------------------------------------------------------------------------------------------------------------------------

		protected List< TreeNode > mTree { get; set; }
	}
}
