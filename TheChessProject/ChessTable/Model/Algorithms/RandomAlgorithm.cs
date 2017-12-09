using ChessTable.Common;
using System;
using System.Collections.Generic;

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
			Int32 randInd			= random.Next( mTreeRoot.childNodes.Count );
			Move randomMove			= mTreeRoot.childNodes[ randInd ].move;

			return randomMove;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override void refreshTree( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures, Move aLastMove )
		{
			setTree( aChessBoard, aWhiteFigures, aBlackFigures );
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override void setTree( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures )
		{
			mTreeRoot = new TreeNode( null );
			List< Int32 > possibleMovesOfTheItem;
			List< ModelItem > myFigures = ( myColor == Colors.WHITE ? aWhiteFigures : aBlackFigures );

			foreach ( ModelItem item in myFigures )
			{
				ModelItem modelItem = new ModelItem( item.x, item.y, item.figureItem.color, item.figureItem.figureType );
				possibleMovesOfTheItem = possibleMoves( aChessBoard, aBlackFigures, aWhiteFigures, modelItem );
				addNewTreeNode( mTreeRoot, modelItem, possibleMovesOfTheItem, aChessBoard );
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		private void addNewTreeNode( TreeNode currentNode, ModelItem aItem, List< Int32 > aPossibleMoves, List< List< ModelItem > > aChessBoard )
		{
			ModelItem EnPassant = new ModelItem();
			Boolean isFindEnPassant = findEnPassantPawn( aChessBoard, ref EnPassant );
			Colors EnPassantColor = Colors.NO_COLOR;
			if ( isFindEnPassant )
			{
				EnPassantColor = EnPassant.figureItem.color;
				EnPassant.figureItem.figureType = FigureType.NO_FIGURE;
				EnPassant.figureItem.color		= Colors.NO_COLOR;
			}

			ModelItem tempItem;
			ModelItem targetItem;
			foreach ( Int32 index in aPossibleMoves )
			{
				ModelItem RookItem = new ModelItem();
				ModelItem newEnPassantItem = new ModelItem();
				
				targetItem		= aChessBoard[ index / 8 ][ index % 8 ];
				tempItem		= new ModelItem( targetItem.x, targetItem.y, targetItem.figureItem.color, targetItem.figureItem.figureType );
				aChessBoard[ aItem.x ][ aItem.y ].figureItem.color			= Colors.NO_COLOR;
				aChessBoard[ aItem.x ][ aItem.y ].figureItem.figureType		= FigureType.NO_FIGURE;

				targetItem.figureItem.color			= aItem.figureItem.color;
				targetItem.figureItem.figureType	= aItem.figureItem.figureType;
				switch( aItem.figureItem.figureType )
				{
				case FigureType.KING:
					{
						targetItem.figureItem.figureType	= FigureType.MOVED_KING;
						if ( Math.Abs( targetItem.x - aItem.x ) == 2 )
						{
							switch ( targetItem.y )
							{
							case 1:
								{
									aChessBoard[ aItem.x ][ 0 ].figureItem = new FigureItem( Colors.NO_COLOR, FigureType.NO_FIGURE );
									RookItem = aChessBoard[ aItem.x ][ 2 ];
								} break;
							case 2:
								{
									aChessBoard[ aItem.x ][ 0 ].figureItem = new FigureItem( Colors.NO_COLOR, FigureType.NO_FIGURE );
									RookItem = aChessBoard[ aItem.x ][ 3 ];
								} break;
							case 5:
								{
									aChessBoard[ aItem.x ][ 7 ].figureItem = new FigureItem( Colors.NO_COLOR, FigureType.NO_FIGURE );
									RookItem = aChessBoard[ aItem.x ][ 4 ];
								} break;
							case 6:
								{
									aChessBoard[ aItem.x ][ 7 ].figureItem = new FigureItem( Colors.NO_COLOR, FigureType.NO_FIGURE );
									RookItem = aChessBoard[ aItem.x ][ 5 ];
								} break;
							}
							RookItem.figureItem = new FigureItem( aItem.figureItem.color, FigureType.MOVED_ROOK );
						}
					} break;
				case FigureType.ROOK:
					{
						targetItem.figureItem.figureType	= FigureType.MOVED_ROOK;
					} break;
				case FigureType.PAWN:
					{
						if ( Math.Abs( targetItem.x - aItem.x ) == 2 )
						{
							Int32 enPassantX = ( aItem.x == 1 ? 2 : 5 );
							newEnPassantItem = aChessBoard[ enPassantX ][ aItem.y ];
							newEnPassantItem.figureItem.color = aItem.figureItem.color;
							newEnPassantItem.figureItem.figureType = FigureType.EN_PASSANT_PAWN;
						}

					} break;
				}

				//------------------------------------------------

				if ( aItem.figureItem.figureType == FigureType.PAWN && ( tempItem.x == 0 || tempItem.x == 7 ) )
				{
					for ( int i = 0; i < 4; i++ )
					{
						switch ( i )
						{
						case 0: targetItem.figureItem.figureType = FigureType.KNIGHT; break;
						case 1: targetItem.figureItem.figureType = FigureType.BISHOP; break;
						case 2: targetItem.figureItem.figureType = FigureType.MOVED_ROOK; break;
						case 3: targetItem.figureItem.figureType = FigureType.QUEEN; break;
						}
						currentNode.childNodes.Add( new TreeNode( currentNode, new Move( aItem, tempItem ) ) );
					}
				}
				else
				{
					currentNode.childNodes.Add( new TreeNode( currentNode, new Move( aItem, tempItem ) ) );
				}

				//------------------------------------------------

				if ( newEnPassantItem.figureItem.figureType == FigureType.EN_PASSANT_PAWN )
				{
					newEnPassantItem.figureItem = new FigureItem( Colors.NO_COLOR, FigureType.NO_FIGURE );
				}
				
				if ( RookItem.figureItem.figureType == FigureType.MOVED_ROOK )
				{
					if ( RookItem.y == 3 || RookItem.y == 4 )
					{
						aChessBoard[ aItem.x ][ 0 ].figureItem = new FigureItem( aItem.figureItem.color, FigureType.ROOK );
						RookItem.figureItem = new FigureItem( Colors.NO_COLOR, FigureType.NO_FIGURE );
					}
					else
					{
						aChessBoard[ aItem.x ][ 7 ].figureItem = new FigureItem( aItem.figureItem.color, FigureType.ROOK );
						RookItem.figureItem = new FigureItem( Colors.NO_COLOR, FigureType.NO_FIGURE );
					}
				}

				aChessBoard[ aItem.x ][ aItem.y ].figureItem.color			= aItem.figureItem.color;
				aChessBoard[ aItem.x ][ aItem.y].figureItem.figureType		= aItem.figureItem.figureType;
				targetItem.figureItem.color			= tempItem.figureItem.color;
				targetItem.figureItem.figureType	= tempItem.figureItem.figureType;
			}

			if ( isFindEnPassant )
			{
				EnPassant.figureItem.figureType = FigureType.EN_PASSANT_PAWN;
				EnPassant.figureItem.color		= EnPassantColor;
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		Boolean findEnPassantPawn( List< List< ModelItem > > aChessBoard, ref ModelItem aEnPassant )
		{
			foreach ( var row in aChessBoard )
			{
				foreach ( var modelItem in row )
				{
					if ( modelItem.figureItem.figureType == FigureType.EN_PASSANT_PAWN )
					{
						aEnPassant = modelItem;
						return true;
					}
				}
			}

			return false;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------
	}
}
