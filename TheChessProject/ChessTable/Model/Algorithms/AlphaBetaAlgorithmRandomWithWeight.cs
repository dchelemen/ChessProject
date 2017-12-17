using ChessTable.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessTable.Model.Algorithms
{
	class AlphaBetaAlgorithmRandomWithWeight : AlphaBetaBaseAlgorithm
	{
		public AlphaBetaAlgorithmRandomWithWeight( Colors aPlayer1Color, Colors aMyColor ) : base( aPlayer1Color, aMyColor )
		{

		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override Move move( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures )
		{
			TreeNode bestNode = new TreeNode();
			List< Move > moves = new List< Move >();
			bestNode.moveValue = -100;
			foreach ( var node in mTreeRoot.childNodes )
			{
				if ( node.moveValue > bestNode.moveValue )
				{
					moves.Clear();
					bestNode = node;
					moves.Add( node.move );
				}
				else if ( node.moveValue == bestNode.moveValue )
				{
					moves.Add( node.move );
				}
			}

			Random rnd = new Random();
			Int32 randomMove = rnd.Next( moves.Count );
			return moves[ randomMove ];
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		protected override void createTreeNode( List< List< ModelItem > > aChessBoard, ModelItem tempItem, ModelItem aItem, Int32 enPassantValue, TreeNode currentNode, Int32 aCurrentDepth, ref Boolean cut )
		{
			TreeNode lastChild;
			if ( aCurrentDepth == mMaxDepth - 1 )
			{
				currentNode.childNodes.Add( new TreeNode( currentNode, new Move( aItem, tempItem ) ) );
				lastChild = currentNode.childNodes[ currentNode.childNodes.Count - 1 ];
				lastChild.moveValue = getMoveValue( aChessBoard, aItem, tempItem, enPassantValue );
			}
			else
			{
				currentNode.childNodes.Add( new TreeNode( currentNode, new Move( aItem, tempItem ) ) );
				lastChild = currentNode.childNodes[ currentNode.childNodes.Count - 1 ];
				Colors nextColor = ( aItem.figureItem.color == Colors.WHITE ? Colors.BLACK : Colors.WHITE );
				nextDepth( lastChild, aChessBoard, aCurrentDepth + 1, nextColor );

				lastChild.countAlphaBetaValue = countAlphaBetaValue( lastChild );
				lastChild.getMoveValue = getMoveValue( aChessBoard, aItem, tempItem, enPassantValue );
				lastChild.depthValue = ( lastChild.getMoveValue != 0 ? ( mMaxDepth - aCurrentDepth ) : 0 );
				if ( myColor == tempItem.figureItem.color )
				{
					lastChild.depthValue *= ( -1 );
				}
				lastChild.moveValue = lastChild.countAlphaBetaValue + lastChild.getMoveValue + lastChild.depthValue;
						
			}
			cut = shouldCut( lastChild );
		}

		//----------------------------------------------------------------------------------------------------------------------------------------
	}
}
