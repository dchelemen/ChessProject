using ChessTable.Common;
using System;
using System.Collections.Generic;

namespace ChessTable.Model.Algorithms
{
	class AlphaBetaAlgorithm : BaseAlgorithm
	{
		public AlphaBetaAlgorithm( Colors aPlayer1Color, Colors aMyColor ) : base( aPlayer1Color, aMyColor )
		{
			isActive = true;
			mMaxDepth = 3;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override void move( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures )
		{
			Int16[] tablePosition	= bestMove();

			aWhiteFigures.Clear();
			aBlackFigures.Clear();
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
					else if( modelItem.figureItem.color == Colors.BLACK )
					{
						aBlackFigures.Add( new ModelItem( modelItem.x, modelItem.y, modelItem.figureItem.color, modelItem.figureItem.figureType ) );
					}

					index++;
				}
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override void refreshTree( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures, Move aLastMove )
		{
			setTree( aChessBoard, aWhiteFigures, aBlackFigures );
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override void setTree( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures )
		{
			depthScores = new List< Int32 >( 3 );
			for ( Int32 ind = 0; ind < depthScores.Count; ind++ )
			{
				depthScores[ ind ] = -1000;
			}

			mTreeRoot = new TreeNode();
			List< Int32 > possibleMovesOfTheItem;
			List< ModelItem > myFigures = ( myColor == Colors.WHITE ? aWhiteFigures : aBlackFigures );

			foreach ( ModelItem item in myFigures )
			{
				ModelItem modelItem = new ModelItem( item.x, item.y, item.figureItem.color, item.figureItem.figureType );
				possibleMovesOfTheItem = possibleMoves( aChessBoard, aBlackFigures, aWhiteFigures, modelItem );
				addNewTreeNode( mTreeRoot, modelItem, possibleMovesOfTheItem, aChessBoard, 0 );
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		private void addNewTreeNode( TreeNode currentNode, ModelItem aItem, List< Int32 > aPossibleMoves, List< List< ModelItem > > aChessBoard, Int32 aCurrentDepth )
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

			Boolean cut = false;
			ModelItem tempItem;
			ModelItem targetItem;
			foreach ( Int32 index in aPossibleMoves )
			{
				if ( cut )
				{
					break;
				}

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
					for ( int i = 0; i < 4 && cut == false; i++ )
					{
						Int32 moveValue = 0;
						switch ( i )
						{
						case 0:
							{
								targetItem.figureItem.figureType = FigureType.KNIGHT;
								moveValue = 3;
							} break;
						case 1:
							{
								targetItem.figureItem.figureType = FigureType.BISHOP;
								moveValue = 3;
							} break;
						case 2:
							{
								targetItem.figureItem.figureType = FigureType.MOVED_ROOK;
								moveValue = 5;
							} break;
						case 3:
							{
								targetItem.figureItem.figureType = FigureType.QUEEN;
								moveValue = 9;
							} break;
						}

						if ( myColor == tempItem.figureItem.color )
						{
							moveValue *= ( -1 );
						}

						TreeNode lastChild;
						Int16[] tablePositions	= getTablePosition( aChessBoard );
						if ( aCurrentDepth == mMaxDepth - 1 )
						{
							currentNode.childNodes.Add( new TreeNode( currentNode, tablePositions ) );
							lastChild = currentNode.childNodes[ currentNode.childNodes.Count - 1 ];
							lastChild.moveValue = moveValue;
						}
						else
						{
							currentNode.childNodes.Add( new TreeNode( currentNode, tablePositions ) );
							lastChild = currentNode.childNodes[ currentNode.childNodes.Count - 1 ];
							Colors nextColor = ( aItem.figureItem.color == Colors.WHITE ? Colors.BLACK : Colors.WHITE );
							nextDepth( lastChild, aChessBoard, aCurrentDepth + 1, nextColor );

							lastChild.countAlphaBetaValue = countAlphaBetaValue( lastChild );
							lastChild.getMoveValue = moveValue;
							lastChild.depthValue = ( lastChild.getMoveValue != 0 ? ( mMaxDepth - aCurrentDepth ) : 0 );
							if ( myColor == tempItem.figureItem.color )
							{
								lastChild.depthValue *= ( -1 );
							}
							lastChild.moveValue = lastChild.countAlphaBetaValue + lastChild.getMoveValue + lastChild.depthValue;
						
						}
						cut = shouldCut( lastChild );
					}
				}
				else
				{
					TreeNode lastChild;
					Int16[] tablePositions	= getTablePosition( aChessBoard );
					if ( aCurrentDepth == mMaxDepth - 1 )
					{
						currentNode.childNodes.Add( new TreeNode( currentNode, tablePositions ) );
						lastChild = currentNode.childNodes[ currentNode.childNodes.Count - 1 ];
						lastChild.moveValue = getMoveValue( aItem, tempItem );
					}
					else
					{
						currentNode.childNodes.Add( new TreeNode( currentNode, tablePositions ) );
						lastChild = currentNode.childNodes[ currentNode.childNodes.Count - 1 ];
						Colors nextColor = ( aItem.figureItem.color == Colors.WHITE ? Colors.BLACK : Colors.WHITE );
						nextDepth( lastChild, aChessBoard, aCurrentDepth + 1, nextColor );

						lastChild.countAlphaBetaValue = countAlphaBetaValue( lastChild );
						lastChild.getMoveValue = getMoveValue( aItem, tempItem );
						lastChild.depthValue = ( lastChild.getMoveValue != 0 ? ( mMaxDepth - aCurrentDepth ) : 0 );
						if ( myColor == tempItem.figureItem.color )
						{
							lastChild.depthValue *= ( -1 );
						}
						lastChild.moveValue = lastChild.countAlphaBetaValue + lastChild.getMoveValue + lastChild.depthValue;
						
					}
					cut = shouldCut( lastChild );
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

		Int16[] getTablePosition( List< List< ModelItem > > aChessBoard )
		{
			Int16[] tablePositions = new Int16[ 64 ];
			Int32 index = 0;
			foreach ( var row in aChessBoard )
			{
				foreach ( var modelItem in row )
				{
					tablePositions[ index ] = getFigureID( modelItem );
					index++;
				}
			}
			return tablePositions;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		void nextDepth( TreeNode aParent, List< List< ModelItem > > aChessBoard, Int32 aCurrentDepth, Colors currentColor )
		{
			List< ModelItem > blackFigures	= new List< ModelItem >();
			List< ModelItem > whiteFigures	= new List< ModelItem >();
			List< ModelItem > myFigures;

			foreach ( var row in aChessBoard )
			{
				foreach ( var modelItem in row )
				{
					if ( modelItem.figureItem.color == Colors.WHITE )
					{
						whiteFigures.Add( new ModelItem( modelItem.x, modelItem.y, modelItem.figureItem.color, modelItem.figureItem.figureType ) );
					}
					else if( modelItem.figureItem.color == Colors.BLACK  )
					{
						blackFigures.Add( new ModelItem( modelItem.x, modelItem.y, modelItem.figureItem.color, modelItem.figureItem.figureType ) );
					}
				}
			}

			myFigures = ( currentColor == Colors.WHITE ? whiteFigures : blackFigures );
			List< Int32 > possibleMovesOfTheItem;
			foreach ( ModelItem item in myFigures )
			{
				ModelItem modelItem = new ModelItem( item.x, item.y, item.figureItem.color, item.figureItem.figureType );
				possibleMovesOfTheItem = possibleMoves( aChessBoard, blackFigures, whiteFigures, modelItem );
				addNewTreeNode( aParent, modelItem, possibleMovesOfTheItem, aChessBoard, aCurrentDepth );
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		Int32 getMoveValue( ModelItem aMover, ModelItem aTarget )
		{
			Int32 returnValue = 0;
			switch ( aTarget.figureItem.figureType )
			{
			case FigureType.PAWN:			returnValue = 1; break;
			case FigureType.KNIGHT:			returnValue = 3; break;
			case FigureType.BISHOP:			returnValue = 3; break;
			case FigureType.ROOK:			returnValue = 5; break;
			case FigureType.MOVED_ROOK:		returnValue = 5; break;
			case FigureType.QUEEN:			returnValue = 9; break;
			case FigureType.KING:			returnValue = 10; break;
			case FigureType.MOVED_KING:		returnValue = 10; break;
			}

			if ( aTarget.figureItem.figureType == FigureType.EN_PASSANT_PAWN && aMover.figureItem.figureType == FigureType.PAWN )
			{
				returnValue = 1;
			}

			if ( myColor == aTarget.figureItem.color )
			{
				returnValue *= ( -1 );
			}
			return returnValue;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		Int16[] bestMove()
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
			return bestNode.tablePosition;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		Int32 countAlphaBetaValue( TreeNode currentNode )
		{
			Int32 returnValue;
			if ( currentNode.player == TreeNode.Player.ALPHA )
			{
				returnValue = max( currentNode.childNodes );
			}
			else
			{
				returnValue = min( currentNode.childNodes );
			}

			return returnValue;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		Int32 min( List< TreeNode > childs )
		{
			Int32 min = 50;
			foreach ( var i in childs )
			{
				if ( i.moveValue < min )
					min = i.moveValue;
			}
			min = ( min != -50 ? min : 0 );
			return min;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		Int32 max( List< TreeNode > childs )
		{
			Int32 max = -50;
			foreach ( var i in childs )
			{
				if ( i.moveValue > max )
					max = i.moveValue;
			}
			max = ( max != -50 ? max : 0 );
			return max;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		Boolean shouldCut( TreeNode lastChild )
		{
			Int32 maxAlpha = -100;
			Int32 minBeta = -100;

			for ( TreeNode p = lastChild; p != null; p = p.parent )
			{
				if ( p.player == TreeNode.Player.ALPHA )
				{
					if ( p.moveValue != -100 && ( maxAlpha != -100 && maxAlpha < p.moveValue || maxAlpha == -100 ) )
					{
						maxAlpha = p.moveValue;
					}
				}
				else
				{
					if ( p.moveValue != -100 && ( minBeta != -100 && minBeta > p.moveValue || minBeta == -100 ) )
					{
						minBeta = p.moveValue;
					}
				}
			}

			return ( maxAlpha != -100 && minBeta != -100 && maxAlpha >= minBeta );
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		private List< Int32 > depthScores { get; set; }
		private Int32 depth { get; set; }

		//-----------------------------------------------------------------------------------------------------------------------------------------

		private readonly Int32 mMaxDepth;
	}
}
