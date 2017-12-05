using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTable.Model.Algorithms
{
	public class TreeNode
	{
		public enum Player
		{
			ALPHA,
			BETA
		};

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public TreeNode( TreeNode aParent = null )
		{
			if ( aParent == null )
			{
				player = Player.ALPHA;
			}
			else
			{
				player = ( aParent.player == Player.ALPHA ? Player.BETA : Player.ALPHA );
			}

			parent			= aParent;
			moveValue		= -100;
			childNodes		= new List< TreeNode >();
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public TreeNode( TreeNode aParent, Int16[] aTablePosition, Int32 aMoveValue = -100 )
		{
			player			= ( aParent.player == Player.ALPHA ? Player.BETA : Player.ALPHA );
			parent			= aParent;
			tablePosition	= aTablePosition;
			moveValue		= aMoveValue;
			childNodes		= new List< TreeNode >();
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

		public TreeNode							parent { get; set; }

		public Player							player { get; set; }

		public List< TreeNode >					childNodes { get; set; }
	}
}
