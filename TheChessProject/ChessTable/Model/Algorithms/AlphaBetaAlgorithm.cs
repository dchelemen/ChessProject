using ChessTable.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessTable.Model.Algorithms
{
	class AlphaBetaAlgorithm : BaseAlgorithm
	{
		public AlphaBetaAlgorithm( Colors aPlayer1Color, Colors aMyColor ) : base( aPlayer1Color, aMyColor )
		{
			isActive = true;
			mMaxDepth = ( aMyColor == Colors.WHITE ? figureValues.whiteDepth : figureValues.blackDepth );
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
								moveValue = figureValues.knightValue;
							} break;
						case 1:
							{
								targetItem.figureItem.figureType = FigureType.BISHOP;
								moveValue = figureValues.bishopValue;
							} break;
						case 2:
							{
								targetItem.figureItem.figureType = FigureType.MOVED_ROOK;
								moveValue = figureValues.rookValue;
							} break;
						case 3:
							{
								targetItem.figureItem.figureType = FigureType.QUEEN;
								moveValue = figureValues.queenValue;
							} break;
						}

						if ( myColor == tempItem.figureItem.color )
						{
							moveValue *= ( -1 );
						}

						TreeNode lastChild;
						if ( aCurrentDepth == mMaxDepth - 1 )
						{
							currentNode.childNodes.Add( new TreeNode( currentNode, new Move( aItem, tempItem ) ) );
							lastChild = currentNode.childNodes[ currentNode.childNodes.Count - 1 ];
							lastChild.moveValue = getMoveValue( aChessBoard, aItem, tempItem, moveValue );
						}
						else
						{
							currentNode.childNodes.Add( new TreeNode( currentNode, new Move( aItem, tempItem ) ) );
							lastChild = currentNode.childNodes[ currentNode.childNodes.Count - 1 ];
							Colors nextColor = ( aItem.figureItem.color == Colors.WHITE ? Colors.BLACK : Colors.WHITE );
							nextDepth( lastChild, aChessBoard, aCurrentDepth + 1, nextColor );

							lastChild.countAlphaBetaValue = countAlphaBetaValue( lastChild );
							lastChild.getMoveValue = getMoveValue( aChessBoard, aItem, tempItem, moveValue );
							lastChild.moveValue = lastChild.countAlphaBetaValue + lastChild.getMoveValue;
						
						}
						cut = shouldCut( lastChild );
					}
				}
				else
				{
					TreeNode lastChild;
					if ( aCurrentDepth == mMaxDepth - 1 )
					{
						currentNode.childNodes.Add( new TreeNode( currentNode, new Move( aItem, tempItem ) ) );
						lastChild = currentNode.childNodes[ currentNode.childNodes.Count - 1 ];
						lastChild.moveValue = getMoveValue( aChessBoard, aItem, tempItem );
					}
					else
					{
						currentNode.childNodes.Add( new TreeNode( currentNode, new Move( aItem, tempItem ) ) );
						lastChild = currentNode.childNodes[ currentNode.childNodes.Count - 1 ];
						Colors nextColor = ( aItem.figureItem.color == Colors.WHITE ? Colors.BLACK : Colors.WHITE );
						nextDepth( lastChild, aChessBoard, aCurrentDepth + 1, nextColor );

						lastChild.countAlphaBetaValue = countAlphaBetaValue( lastChild );
						lastChild.getMoveValue = getMoveValue( aChessBoard, aItem, tempItem );
						lastChild.moveValue = lastChild.countAlphaBetaValue + lastChild.getMoveValue;
						
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

		Int32 getMoveValue( List< List< ModelItem > > aChessBoard, ModelItem aMover, ModelItem aTarget, Int32 fixValue = 0 )
		{
			Int32 returnValue = 0;

			if ( fixValue == 0 )
			{
				switch ( aTarget.figureItem.figureType )
				{
				case FigureType.PAWN:			returnValue = figureValues.pawnValue;	break;
				case FigureType.KNIGHT:			returnValue = figureValues.knightValue; break;
				case FigureType.BISHOP:			returnValue = figureValues.bishopValue; break;
				case FigureType.ROOK:			returnValue = figureValues.rookValue;	break;
				case FigureType.MOVED_ROOK:		returnValue = figureValues.rookValue;	break;
				case FigureType.QUEEN:			returnValue = figureValues.queenValue;	break;
				case FigureType.KING:			returnValue = figureValues.kingValue;	break;
				case FigureType.MOVED_KING:		returnValue = figureValues.kingValue;	break;
				}

				if ( aTarget.figureItem.figureType == FigureType.EN_PASSANT_PAWN && aMover.figureItem.figureType == FigureType.PAWN )
				{
					returnValue = figureValues.pawnValue;
				}
			}
			else
			{
				returnValue = fixValue;
			}

			Int32 isCheckMatOrCanNotMove = isCheckMat( aChessBoard, aMover );

			if ( isCheckMatOrCanNotMove != 0 )
			{
				returnValue = isCheckMatOrCanNotMove;
			}

			if ( myColor == aTarget.figureItem.color )
			{
				returnValue *= ( -1 );
			}
			return returnValue;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		Int32 isCheckMat( List< List< ModelItem > > aChessBoard, ModelItem aMover )
		{
			Colors enemyColor = ( aMover.figureItem.color == Colors.WHITE ? Colors.BLACK : Colors.WHITE );

			List< ModelItem > blackFigures	= new List< ModelItem >();
			List< ModelItem > whiteFigures	= new List< ModelItem >();
			List< ModelItem > enemyFigures;
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

			if ( enemyColor == Colors.WHITE )
			{
				enemyFigures	= whiteFigures;
				myFigures		= blackFigures;
			}
			else
			{
				enemyFigures	= blackFigures;
				myFigures		= whiteFigures;
			}

			List< Int32 > possibleMovesOfTheItem;
			Boolean isEnemyCanNotMove = true;
			foreach ( ModelItem item in enemyFigures )
			{
				ModelItem modelItem = new ModelItem( item.x, item.y, item.figureItem.color, item.figureItem.figureType );
				possibleMovesOfTheItem = possibleMoves( aChessBoard, blackFigures, whiteFigures, modelItem );
				if ( possibleMovesOfTheItem.Count != 0 )
				{
					isEnemyCanNotMove = false;
					break;
				}
			}

			Boolean isCheck = false;
			if ( isEnemyCanNotMove )
			{
				Int32 enemyKingPosition = enemyFigures.Where( X => ( X.figureItem.figureType == FigureType.KING || X.figureItem.figureType == FigureType.MOVED_KING ) ).FirstOrDefault().index;
				foreach ( ModelItem item in myFigures )
				{
					ModelItem modelItem = new ModelItem( item.x, item.y, item.figureItem.color, item.figureItem.figureType );
					possibleMovesOfTheItem = possibleMoves( aChessBoard, blackFigures, whiteFigures, modelItem );
					if ( possibleMovesOfTheItem.Contains( enemyKingPosition ) )
					{
						isCheck = true;
						break;
					}
				}
			}

			if ( isEnemyCanNotMove && isCheck )
			{
				return figureValues.checkmatValue;
			}
			else if ( isEnemyCanNotMove && ! isCheck )
			{
				return figureValues.drawValue;
			}
			else
			{
				return 0;
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		Move bestMove()
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
