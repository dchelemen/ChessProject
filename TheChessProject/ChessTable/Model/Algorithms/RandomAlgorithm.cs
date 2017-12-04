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

		public override Move move( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures )
		{
			Random random			= new Random();
			Int32 randInd			= random.Next( mTree.Count );
			Int16[] tablePosition	= mTree[ randInd ].tablePosition;
			Move lastMove			= mTree[ randInd ].move;
			aWhiteFigures.Clear();

			Int32 index = 0;
			foreach ( var row in aChessBoard )
			{
				foreach ( var modelItem in row )
				{
					modelItem.figureItem = getFigureItemFromPosition( tablePosition[ index ] );
					if ( modelItem.figureItem.color == Colors.WHITE )
					{
						aWhiteFigures.Add( new ModelItem( modelItem.x, modelItem.y, modelItem.figureItem.color, modelItem.figureItem.figureType ) );
					}
					else
					{
						aBlackFigures.Add( new ModelItem( modelItem.x, modelItem.y, modelItem.figureItem.color, modelItem.figureItem.figureType ) );
					}

					index++;
				}
			}

			return lastMove;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override void refreshTree( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures, Move aLastMove )
		{
			setTree( aChessBoard, aWhiteFigures, aBlackFigures );
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override void setTree( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures )
		{
			mTree = new List< TreeNode >();

			List< Int32 > possibleMovesOfTheItem;
			List< ModelItem > myFigures = ( myColor == Colors.WHITE ? aWhiteFigures : aBlackFigures );

			foreach ( ModelItem item in myFigures )
			{
				possibleMovesOfTheItem = possibleMoves( aChessBoard, aBlackFigures, aWhiteFigures, item );
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
	}
}
