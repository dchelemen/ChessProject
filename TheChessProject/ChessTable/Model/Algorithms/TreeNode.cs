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

		public Int32							MoveFrom { get; set; }
		public Int32							MoveValue { get; set; }

		public List< TreeNode >					childNodes { get; set; }
	}
}
