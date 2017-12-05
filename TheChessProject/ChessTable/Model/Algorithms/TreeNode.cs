using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTable.Model.Algorithms
{
	public class TreeNode
	{
		public TreeNode(){}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public TreeNode( Int16[] aTablePosition, Int32 aMoveValue = 0 )
		{
			tablePosition	= aTablePosition;
			moveValue		= aMoveValue;
			childNodes		= null;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public void addChild( TreeNode aChild )
		{
			if ( childNodes == null )
			{
				childNodes = new List<TreeNode>();
			}

			childNodes.Add( aChild );
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public Int16[]							tablePosition { get; set; }
		public Int32							moveValue { get; set; }
		public Move								move { get; set; }

		public List< TreeNode >					childNodes { get; set; }
	}
}
