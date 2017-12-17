﻿using ChessTable.Common;
using System;
using System.Collections.Generic;

namespace ChessTable.Model.Algorithms
{
	class AlphaBetaAlgorithm : AlphaBetaBaseAlgorithm
	{
		public AlphaBetaAlgorithm( Colors aPlayer1Color, Colors aMyColor ) : base( aPlayer1Color, aMyColor )
		{
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override Move move( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures )
		{
			TreeNode bestNode = new TreeNode();
			bestNode.moveValue = -100;
			foreach ( var node in mTreeRoot.childNodes )
			{
				if ( node.moveValue > bestNode.moveValue )
				{
					bestNode = node;
				}
			}

			return bestNode.move;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

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
				lastChild.moveValue = lastChild.countAlphaBetaValue + lastChild.getMoveValue;
						
			}
			cut = shouldCut( lastChild );
		}
	}
}
