using ChessTable.Common;
using System;
using System.Linq;
using System.Collections.Generic;

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
									index	= -1,
									type	= new Tuple< Colors, FigureType >( Colors.NO_COLOR, FigureType.NO_FIGURE ),
									x		= -1,
									y		= -1
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
						index	= row * 8 + column,
						x		= row,
						y		= column,
						type	= new Tuple< Colors, FigureType >( Colors.NO_COLOR, FigureType.NO_FIGURE )
					} );
				}
				chessBoard.Add( chessBoardRow );
			}

			foreach ( ModelItem item in whiteFigures )
			{
				chessBoard[ item.x ][ item.y ] = item;
				fieldClicked( this, new PutFigureOnTheTableEventArg
				{
					type	= item.type,
					x		= item.x,
					y		= item.y,
					index	= item.index
				} );
			}

			foreach ( ModelItem item in blackFigures )
			{
				chessBoard[ item.x ][ item.y ] = item;
				fieldClicked( this, new PutFigureOnTheTableEventArg
				{
					type	= item.type,
					x		= item.x,
					y		= item.y,
					index	= item.index
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
			if ( aFigureToMove.type.Item1 != mCurrentColor )
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
			setHighlight( this, new SetHighlightEventArg
			{
				index = mFigureToMove.index,
				color = Colors.NO_COLOR
			} );

			mIsFirstClick = true;

			if ( aPlaceHere.type.Item1 == mCurrentColor )
			{
				return;
			}

			if ( aPlaceHere.type.Item1 != Colors.NO_COLOR )
			{
				if ( aPlaceHere.type.Item1 == Colors.WHITE )
				{
					ModelItem oldItem = whiteFigures.Where( X => X.index == aPlaceHere.index ).FirstOrDefault();
					whiteFigures.Remove( oldItem );
				}
				else
				{
					ModelItem oldItem = blackFigures.Where( X => X.index == aPlaceHere.index ).FirstOrDefault();
					blackFigures.Remove( oldItem );
				}
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
				index	= mFigureToMove.index,
				type	= new Tuple<Colors, FigureType>(Colors.NO_COLOR, FigureType.NO_FIGURE),
				x		= mFigureToMove.x,
				y		= mFigureToMove.y,
			} );
			chessBoard[ mFigureToMove.x ][ mFigureToMove.y ].type = new Tuple<Colors, FigureType>( Colors.NO_COLOR, FigureType.NO_FIGURE );

			fieldClicked( this, new PutFigureOnTheTableEventArg
			{
				index	= aPlaceHere.index,
				type	= mFigureToMove.type,
				x		= aPlaceHere.x,
				y		= aPlaceHere.y,
			} );
			chessBoard[ aPlaceHere.x ][ aPlaceHere.y ].type = mFigureToMove.type;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private void setPossibleMoves()
		{
			possibleMoves.Clear();
			switch ( mFigureToMove.type.Item2 )
			{
				case FigureType.KING: break;
				case FigureType.QUEEN: break;
				case FigureType.ROOK: break;
				case FigureType.BISHOP: break;
				case FigureType.KNIGHT: break;
				case FigureType.PAWN: setPawnMoves(); break;
				case FigureType.NO_FIGURE: break;
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private void setPawnMoves()
		{
			Int32 x, y;
			if ( mCurrentColor == mPlayer1Color )
			{
				if ( mFigureToMove.x == 6 )
				{
					x = mFigureToMove.x - 2;
					y = mFigureToMove.y;

					if ( isTargetRight( x, y ) )
					{
						possibleMoves.Add( new ModelItem
						{
							x		= x,
							y		= y,
							index	= x * 8 + y,
						} );
					}
				}

				x = mFigureToMove.x - 1;
				y = mFigureToMove.y;

				if ( isTargetRight( x, y ) )
				{
					possibleMoves.Add( new ModelItem
					{
						x		= x,
						y		= y,
						index	= x * 8 + y,
					} );
				}

				y = mFigureToMove.y - 1;

				if ( isTargetRight( x, y ) )
				{
					possibleMoves.Add( new ModelItem
					{
						x		= x,
						y		= y,
						index	= x * 8 + y,
					} );
				}

				y = mFigureToMove.y + 1;

				if ( isTargetRight( x, y ) )
				{
					possibleMoves.Add( new ModelItem
					{
						x		= x,
						y		= y,
						index	= x * 8 + y,
					} );
				}
			}
			else
			{

			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		Boolean isTargetRight( Int32 aX, Int32 aY )
		{
			if ( aX < 0 || aX > 7 || aY < 0 || aY > 7 )
			{
				return false;
			}

			if ( chessBoard[ aX ][ aY ].type.Item1 == mFigureToMove.type.Item1 )
			{
				return false;
			}

			if ( chessBoard[ aX ][ aY ].type.Item1 == Colors.NO_COLOR )
			{
				setHighlight( this, new SetHighlightEventArg
				{
					index = aX * 8 + aY,
					color = Colors.BLUE
				} );
			}
			else
			{
				setHighlight( this, new SetHighlightEventArg
				{
					index = aX * 8 + aY,
					color = Colors.RED
				} );
			}
			return true;
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
