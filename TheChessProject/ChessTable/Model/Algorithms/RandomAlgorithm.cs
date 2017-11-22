using ChessTable.Common;
using ChessTable.Model.Algorithms;
using ChessTable.Model.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTable.Model.Algorithms
{
	class RandomAlgorithm : BaseAlgorithm
	{
		public RandomAlgorithm( Colors aPlayer1Color, Colors aMyColor ) : base( aPlayer1Color, aMyColor )
		{
			isActive = true;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override Move nextMove( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures, CastlingRule aCastlingRule )
		{
			Random random = new Random();
			Int32 randInd = random.Next( mTree.Count );
			Move temporary = mTree[ randInd ].moveItem;
			Move returnMoveItem = new Move( temporary.itemFrom, temporary.itemTo );

			return new Move( temporary.itemFrom, temporary.itemTo );;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override void refreshTree( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures, CastlingRule aCastlingRule, Move aLastMove )
		{
			setTree( aChessBoard, aWhiteFigures, aBlackFigures, aCastlingRule );
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override void setTree( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures, CastlingRule aCastlingRule )
		{
			mTree = new List< TreeNode >();

			List< Int32 > possibleMovesOfTheItem;
			List< ModelItem > myFigures = ( myColor == Colors.WHITE ? aWhiteFigures : aBlackFigures );

			foreach ( ModelItem item in myFigures )
			{
				possibleMovesOfTheItem = possibleMoves( aChessBoard, aBlackFigures, aWhiteFigures, aCastlingRule, item );
				addNewTreeNode( item, possibleMovesOfTheItem, aChessBoard );
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		private void addNewTreeNode( ModelItem aItem, List< Int32 > aPossibleMoves, List< List< ModelItem > > aChessBoard )
		{
			foreach ( Int32 index in aPossibleMoves )
			{
				Move move = new Move( aItem, aChessBoard[ index / 8 ][ index % 8] );
				mTree.Add( new TreeNode( move ) );
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		private Int32 removeAllFrom( TreeNode aNode )
		{
			Int32 numberOfDeleted = 0;
			for ( Int32 index = mTree.Count - 1; index >= 0; index -- )
			{
				TreeNode node = mTree[ index ];
				if ( node.moveItem.itemFrom == aNode.moveItem.itemFrom )
				{
					mTree.Remove( node );
					numberOfDeleted++;
				}
			}
			return numberOfDeleted;
		}
	}
}
