using ChessTable.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using ChessTable.Model.Rules;

namespace ChessTable.Model
{
	public class ChessBoardModel
	{
		public ChessBoardModel( Colors aPlayer1Color, Colors aStartingColor, Algorithm aPlayer1Algorithm, Algorithm aPlayer2Algorithm )
		{
			mPlayer1Color		= aPlayer1Color;
			mPlayer1Algorithm	= aPlayer1Algorithm;
			mPlayer2Algorithm	= aPlayer2Algorithm;
			mCurrentColor		= aStartingColor;

			whiteFigures		= new List< ModelItem >();
			blackFigures		= new List< ModelItem >();

			isGameReady			= false;
			mIsFirstClick		= true;

			mFigureToMove		= new ModelItem
								{
									index		= -1,
									figureItem	= new FigureItem( Colors.NO_COLOR, FigureType.NO_FIGURE ),
									x			= -1,
									y			= -1
								};
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public void startModel()
		{
			chessBoard		= new List< List< ModelItem > >();
			possibleMoves	= new List< ModelItem >();

			for ( Int32 row = 0; row < 8; row++ )
			{
				List< ModelItem > chessBoardRow = new List< ModelItem >();
				for ( Int32 column = 0; column < 8; column++ )
				{
					chessBoardRow.Add( new ModelItem
					{
						index		= row * 8 + column,
						x			= row,
						y			= column,
						figureItem	= new FigureItem( Colors.NO_COLOR, FigureType.NO_FIGURE )
					} );
				}
				chessBoard.Add( chessBoardRow );
			}

			foreach ( ModelItem item in whiteFigures )
			{
				chessBoard[ item.x ][ item.y ] = item;
				fieldClicked( this, new PutFigureOnTheTableEventArg
				{
					figureItem	= item.figureItem,
					x			= item.x,
					y			= item.y,
					index		= item.index
				} );
			}

			foreach ( ModelItem item in blackFigures )
			{
				chessBoard[ item.x ][ item.y ] = item;
				fieldClicked( this, new PutFigureOnTheTableEventArg
				{
					figureItem	= item.figureItem,
					x			= item.x,
					y			= item.y,
					index		= item.index
				} );
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public void moveFigure( ModelItem aIndex )
		{
			if ( mIsFirstClick )
			{
				moveFigureFrom( aIndex );
			}
			else
			{
				moveFigureTo( aIndex );
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private void moveFigureFrom( ModelItem aFigureToMove )
		{
			if ( aFigureToMove.figureItem.color != mCurrentColor )
			{
				return;
			}

			mFigureToMove = aFigureToMove;

			setPossibleMoves();

			setHighlight( this, new SetHighlightEventArg
			{
				index = aFigureToMove.index,
				color = Colors.BLUE
			} );

			mIsFirstClick = false;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private void moveFigureTo( ModelItem aPlaceHere )
		{
			mIsFirstClick = true;

			if ( aPlaceHere == mFigureToMove )
			{
				removeHighLights();
				return;
			}

			if ( ! possibleMoves.Contains( aPlaceHere ) )
			{
				mIsFirstClick = false;
				return;
			}

			if ( aPlaceHere.figureItem.color == Colors.WHITE )
			{
				//ModelItem oldItem = whiteFigures.Where( X => X.index == aPlaceHere.index ).FirstOrDefault();
				whiteFigures.Remove( aPlaceHere );
			}
			else if ( aPlaceHere.figureItem.color == Colors.BLACK )
			{
				//ModelItem oldItem = blackFigures.Where( X => X.index == aPlaceHere.index ).FirstOrDefault();
				blackFigures.Remove( aPlaceHere );
			}

			ModelItem tempItem;

			if ( mCurrentColor == Colors.WHITE )
			{
				tempItem = whiteFigures.Where( X => X.index == mFigureToMove.index ).FirstOrDefault();
				mCurrentColor = Colors.BLACK;
			}
			else
			{
				tempItem = blackFigures.Where( X => X.index == mFigureToMove.index ).FirstOrDefault();
				mCurrentColor = Colors.WHITE;
			}

			tempItem.index	= aPlaceHere.index;
			tempItem.x		= aPlaceHere.x;
			tempItem.y		= aPlaceHere.y;

			fieldClicked( this, new PutFigureOnTheTableEventArg
			{
				index		= mFigureToMove.index,
				figureItem	= new FigureItem( Colors.NO_COLOR, FigureType.NO_FIGURE ),
				x			= mFigureToMove.x,
				y			= mFigureToMove.y,
			} );
			chessBoard[ mFigureToMove.x ][ mFigureToMove.y ].figureItem = new FigureItem( Colors.NO_COLOR, FigureType.NO_FIGURE );

			fieldClicked( this, new PutFigureOnTheTableEventArg
			{
				index		= aPlaceHere.index,
				figureItem	= mFigureToMove.figureItem,
				x			= aPlaceHere.x,
				y			= aPlaceHere.y,
			} );
			chessBoard[ aPlaceHere.x ][ aPlaceHere.y ].figureItem = mFigureToMove.figureItem;

			removeHighLights();
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private void removeHighLights()
		{
			setHighlight( this, new SetHighlightEventArg // Removing HighLights from the Figure itself
			{
				index = mFigureToMove.index,
				color = Colors.NO_COLOR
			} );

			foreach ( ModelItem fields in possibleMoves ) // Removing HighLights from the possible moves
			{
				setHighlight( this, new SetHighlightEventArg
				{
					index = fields.index,
					color = Colors.NO_COLOR
				} );
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private void setPossibleMoves()
		{
			possibleMoves.Clear();
			switch ( mFigureToMove.figureItem.figureType )
			{
			case FigureType.KING:
				{
					KingRule kingRule		= new KingRule( chessBoard, mPlayer1Color, mFigureToMove );
					possibleMoves			= kingRule.possibleMoves();
				} break;
			case FigureType.QUEEN:
				{
					QueenRule queenRule		= new QueenRule( chessBoard, mPlayer1Color, mFigureToMove );
					possibleMoves			= queenRule.possibleMoves();
				} break;
			case FigureType.ROOK:
				{
					RookRule rookRule		= new RookRule( chessBoard, mPlayer1Color, mFigureToMove );
					possibleMoves			= rookRule.possibleMoves();
				} break;
			case FigureType.BISHOP:
				{
					BishopRule bishopRule	= new BishopRule( chessBoard, mPlayer1Color, mFigureToMove );
					possibleMoves			= bishopRule.possibleMoves();
				} break;
			case FigureType.KNIGHT:
				{
					KnightRule knightRule	= new KnightRule( chessBoard, mPlayer1Color, mFigureToMove );
					possibleMoves			= knightRule.possibleMoves();
				} break;
			case FigureType.PAWN:
				{
					PawnRule pawnRule		= new PawnRule( chessBoard, mPlayer1Color, mFigureToMove );
					possibleMoves			= pawnRule.possibleMoves();
				} break;
			case FigureType.NO_FIGURE:		break;
			}

			foreach ( ModelItem fields in possibleMoves )
			{
				setHighlight( this, new SetHighlightEventArg
				{
					index = fields.index,
					color = Colors.BLUE
				} );
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public event EventHandler< PutFigureOnTheTableEventArg >	fieldClicked;

		public event EventHandler< SetHighlightEventArg >			setHighlight;
		public Boolean												isGameReady { get; set; }
		public List< ModelItem >									whiteFigures { get; set; }
		public List< ModelItem >									blackFigures { get; set; }

		private List< ModelItem >									possibleMoves { get; set; }
		private List< List< ModelItem > >							chessBoard { get; set; }

		//-----------------------------------------------------------------------------------------------------------------------------------------

		private Colors												mPlayer1Color;
		private Algorithm											mPlayer1Algorithm;
		private Algorithm											mPlayer2Algorithm;
		private Colors												mCurrentColor;

		private Boolean												mIsFirstClick;

		private ModelItem											mFigureToMove;
	}
}
