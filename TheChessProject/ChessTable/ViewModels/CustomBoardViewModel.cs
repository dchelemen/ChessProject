﻿using System;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using ChessTable.ViewModels.ImplementedInterfaces;
using ChessTable.Common;
using ChessTable.Model;

namespace ChessTable.ViewModels
{
	public class CustomBoardViewModel : ViewModelBase 
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Constructor for CustomBoardViewModel
		/// </summary>
		public CustomBoardViewModel( Colors aPlayer1Color, Colors aStartingColor, Algorithm aPlayer1Algorithm, Algorithm aPlayer2Algorithm )
		{
			windowState				= "Normal";
			windowWidth				= 640;
			windowHeight			= 480;
			fieldSize				= 48;
			boardSize				= 384;

			isStartBtnClicked		= false;

			selectedPanelItem		= new FigureItem( Colors.NO_COLOR, FigureType.NO_FIGURE );
			mLastClickedField		= -1;

			startBtnClicked			= new DelegateCommand( X => onStartBtnClicked() );
			cancelBtnClicked		= new DelegateCommand( X => onCancelBtnClicked() );
			deleteSelectedClicked	= new DelegateCommand( X => onDeleteSelectedClicked() );

			mChessBoardCollection	= new ObservableCollection< BoardItem >();
			mBlackFigureCollection	= new ObservableCollection< BoardItem >();
			mWhiteFigureCollection	= new ObservableCollection< BoardItem >();
			mChessBoardModel		= new ChessBoardModel( aPlayer1Color, aStartingColor, aPlayer1Algorithm, aPlayer2Algorithm );
			mChessBoardModel.disableCastling();

			setupCustomBoard();
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		private void onStartBtnClicked()
		{
			isStartBtnClicked = true;
			closeCustomBoardView( this, null );
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		private void onCancelBtnClicked()
		{
			closeCustomBoardView( this, null );
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		private void onDeleteSelectedClicked()
		{
			if ( mLastClickedField == -1 )
			{
				return;
			}

			BoardItem selectedItem = mChessBoardCollection[ mLastClickedField ];

			if ( selectedItem.figureItem.color == Colors.WHITE )
			{
				ModelItem oldItem = mChessBoardModel.whiteFigures.Where( X => X.index == selectedItem.Index ).FirstOrDefault();
				mChessBoardModel.whiteFigures.Remove( oldItem );
			}
			else
			{
				ModelItem oldItem = mChessBoardModel.blackFigures.Where( X => X.index == selectedItem.Index ).FirstOrDefault();
				mChessBoardModel.blackFigures.Remove( oldItem );
			}

			mChessBoardCollection[ mLastClickedField ].figureItem		= new FigureItem( Colors.NO_COLOR, FigureType.NO_FIGURE );
			mChessBoardCollection[ mLastClickedField ].highlightColor	= Colors.NO_COLOR;
			mLastClickedField = -1;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// creates the board colors (black and white)
		/// </summary>
		private void setupCustomBoard()
		{
			mChessBoardCollection.Clear();
			mBlackFigureCollection.Clear();
			mWhiteFigureCollection.Clear();

			Colors color;
			Int32 index = 0;
			for ( Int16 row = 0; row < 8; row++ )
			{
				for ( Int16 column = 0; column < 8; column++ )
				{
					color = ( row + column ) % 2 == 0 ? Colors.WHITE : Colors.BLACK;
					mChessBoardCollection.Add(new BoardItem()
					{
						X				= row,
						Y				= column,
						Index			= index,
						fieldColor		= color,
						fieldSize		= 48,
						figureItem		= new FigureItem( Colors.NO_COLOR, FigureType.NO_FIGURE ),
						highlightColor	= Colors.NO_COLOR
					} );
					mChessBoardCollection[ index ].fieldClicked += new EventHandler< FieldClickedEventArg >( onFieldClicked );
					index++;
				}
			}

			for( Int16 row = 0; row < 6; row++ )
			{
				mBlackFigureCollection.Add( new BoardItem
				{
					X					= row,
					Y					= 0, //its for Black
					Index				= row,
					fieldColor			= Colors.BLACK,
					fieldSize			= 48,
					highlightColor		= Colors.NO_COLOR,
					Color				= Colors.BLACK
				} );

				mWhiteFigureCollection.Add( new BoardItem
				{
					X					= row,
					Y					= 1, //its for White
					Index				= row,
					fieldColor			= Colors.BLACK,
					fieldSize			= 48,
					highlightColor		= Colors.NO_COLOR,
					Color				= Colors.WHITE
				} );

				mBlackFigureCollection[ row ].fieldClicked += new EventHandler< FieldClickedEventArg >( onPanelClicked );
				mWhiteFigureCollection[ row ].fieldClicked += new EventHandler< FieldClickedEventArg >( onPanelClicked );
			}

			mBlackFigureCollection[ 0 ].figureItem = new FigureItem( Colors.BLACK ,FigureType.KING   );
			mBlackFigureCollection[ 1 ].figureItem = new FigureItem( Colors.BLACK, FigureType.QUEEN  );
			mBlackFigureCollection[ 2 ].figureItem = new FigureItem( Colors.BLACK, FigureType.ROOK   );
			mBlackFigureCollection[ 3 ].figureItem = new FigureItem( Colors.BLACK, FigureType.BISHOP );
			mBlackFigureCollection[ 4 ].figureItem = new FigureItem( Colors.BLACK, FigureType.KNIGHT );
			mBlackFigureCollection[ 5 ].figureItem = new FigureItem( Colors.BLACK, FigureType.PAWN   );

			mWhiteFigureCollection[ 0 ].figureItem = new FigureItem( Colors.WHITE, FigureType.KING   );
			mWhiteFigureCollection[ 1 ].figureItem = new FigureItem( Colors.WHITE, FigureType.QUEEN  );
			mWhiteFigureCollection[ 2 ].figureItem = new FigureItem( Colors.WHITE, FigureType.ROOK   );
			mWhiteFigureCollection[ 3 ].figureItem = new FigureItem( Colors.WHITE, FigureType.BISHOP );
			mWhiteFigureCollection[ 4 ].figureItem = new FigureItem( Colors.WHITE, FigureType.KNIGHT );
			mWhiteFigureCollection[ 5 ].figureItem = new FigureItem( Colors.WHITE, FigureType.PAWN   );
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		private void onFieldClicked( Object aSender, FieldClickedEventArg aArguments )
		{
			if ( mLastClickedField == aArguments.index ) // Same Field?
			{
				if ( selectedPanelItem.figureType == FigureType.NO_FIGURE )
				{
					mChessBoardCollection[ mLastClickedField ].highlightColor = Colors.NO_COLOR;
					mLastClickedField = -1;
					return;
				}
				if ( aArguments.figureItem == selectedPanelItem ) //Same Figure?
				{
					mChessBoardCollection[ mLastClickedField ].highlightColor = Colors.NO_COLOR;
					mLastClickedField = -1;
				}
				else // We put another Figure instead of it
				{
					if ( mChessBoardCollection[ mLastClickedField ].figureItem.color == selectedPanelItem.color ) // is it from the same Color?
					{
						if ( selectedPanelItem.color == Colors.WHITE ) // Is that White?
						{
							mChessBoardModel.whiteFigures.Where( X => X.index == mLastClickedField ).FirstOrDefault().figureItem = selectedPanelItem;
							mChessBoardCollection[ aArguments.index ].figureItem = selectedPanelItem;
						}
						else // Is That Black?
						{
							mChessBoardModel.blackFigures.Where( X => X.index == mLastClickedField ).FirstOrDefault().figureItem = selectedPanelItem;
							mChessBoardCollection[ aArguments.index ].figureItem = selectedPanelItem;
						}
					}
					else
					{ 
						if ( selectedPanelItem.color == Colors.WHITE ) // Is that White?
						{
							ModelItem oldItem = mChessBoardModel.blackFigures.Where( X => X.index == mLastClickedField ).FirstOrDefault();
							mChessBoardModel.blackFigures.Remove( oldItem );

							mChessBoardModel.whiteFigures.Add( new ModelItem
							{
								figureItem	= selectedPanelItem,
								x			= aArguments.x,
								y			= aArguments.y,
								index		= aArguments.index
							} );
							mChessBoardCollection[ aArguments.index ].figureItem = selectedPanelItem;
						}
						else // is that Black?
						{
							ModelItem oldItem = mChessBoardModel.whiteFigures.Where( X => X.index == mLastClickedField ).FirstOrDefault();
							mChessBoardModel.whiteFigures.Remove( oldItem );

							mChessBoardModel.blackFigures.Add( new ModelItem
							{
								figureItem	= selectedPanelItem,
								x			= aArguments.x,
								y			= aArguments.y,
								index		= aArguments.index
							} );
							mChessBoardCollection[ aArguments.index ].figureItem = selectedPanelItem;
						}
					}
				}
				return;
			}

			if ( selectedPanelItem.figureType == FigureType.NO_FIGURE )
			{
				if ( aArguments.figureItem.figureType != FigureType.NO_FIGURE )
				{
					if ( mLastClickedField != -1 )
					{
						mChessBoardCollection[ mLastClickedField ].highlightColor = Colors.NO_COLOR;
					}

					mChessBoardCollection[ aArguments.index ].highlightColor = Colors.BLUE;
					mLastClickedField = aArguments.index;
				}
				return;
			}

			if ( mLastClickedField != -1 ) // last selected item becomes non selected
			{
				mChessBoardCollection[ mLastClickedField ].highlightColor = Colors.NO_COLOR;
			}
			mLastClickedField = aArguments.index;

			if ( selectedPanelItem == aArguments.figureItem )
			{
				mChessBoardCollection[ aArguments.index ].highlightColor = Colors.RED;
				mLastClickedField = aArguments.index;
				return;
			}

			if ( aArguments.figureItem.figureType == FigureType.NO_FIGURE )
			{
				if ( selectedPanelItem.color == Colors.WHITE )
				{
					mChessBoardModel.whiteFigures.Add( new ModelItem
					{
						figureItem	= selectedPanelItem,
						x			= aArguments.x,
						y			= aArguments.y,
						index		= aArguments.index
					} );
				}
				else
				{
					mChessBoardModel.blackFigures.Add( new ModelItem
					{
						figureItem	= selectedPanelItem,
						x			= aArguments.x,
						y			= aArguments.y,
						index		= aArguments.index
					} );
				}
				mChessBoardCollection[ aArguments.index ].highlightColor	= Colors.RED;
				mChessBoardCollection[ aArguments.index ].figureItem		= selectedPanelItem;
				return;
			}

			if ( selectedPanelItem.color != aArguments.figureItem.color )
			{
				if ( aArguments.figureItem.color == Colors.WHITE )
				{
					ModelItem oldItem = mChessBoardModel.whiteFigures.Where( X => X.index == mLastClickedField ).FirstOrDefault();
					mChessBoardModel.whiteFigures.Remove( oldItem );

					mChessBoardModel.blackFigures.Add( new ModelItem
					{
						figureItem	=  selectedPanelItem,
						x			= aArguments.x,
						y			= aArguments.y,
						index		= aArguments.index
					} );
				}
				else
				{
					ModelItem oldItem = mChessBoardModel.blackFigures.Where( X => X.index == mLastClickedField ).FirstOrDefault();
					mChessBoardModel.blackFigures.Remove( oldItem );

					mChessBoardModel.whiteFigures.Add( new ModelItem
					{
						figureItem	= selectedPanelItem,
						x			= aArguments.x,
						y			= aArguments.y,
						index		= aArguments.index
					} );
				}
				mChessBoardCollection[ aArguments.index ].highlightColor	= Colors.RED;
				mChessBoardCollection[ aArguments.index ].figureItem		= selectedPanelItem;
				return;
			}

			if ( aArguments.figureItem.color == Colors.WHITE )
			{
				mChessBoardModel.whiteFigures.Where( X => X.index == mLastClickedField ).FirstOrDefault().figureItem = selectedPanelItem;
			}
			else
			{
				mChessBoardModel.blackFigures.Where( X => X.index == mLastClickedField ).FirstOrDefault().figureItem = selectedPanelItem;
			}
			mChessBoardCollection[ aArguments.index ].highlightColor	= Colors.RED;
			mChessBoardCollection[ aArguments.index ].figureItem		= selectedPanelItem;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		private void onPanelClicked( Object aSender, FieldClickedEventArg aArguments )
		{
			if ( selectedPanelItem == aArguments.figureItem )
			{
				mBlackFigureCollection.Concat( mWhiteFigureCollection ).Where( X => X.figureItem == selectedPanelItem ).FirstOrDefault().highlightColor = Colors.NO_COLOR;
				selectedPanelItem = new FigureItem( Colors.NO_COLOR, FigureType.NO_FIGURE );
				return;
			}
			if ( selectedPanelItem.figureType != FigureType.NO_FIGURE )
			{
				mBlackFigureCollection.Concat( mWhiteFigureCollection ).Where( X => X.figureItem == selectedPanelItem ).FirstOrDefault().highlightColor = Colors.NO_COLOR;
			}

			selectedPanelItem = aArguments.figureItem;

			mBlackFigureCollection.Concat( mWhiteFigureCollection ).Where( X => X.figureItem == selectedPanelItem ).FirstOrDefault().highlightColor = Colors.RED;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		private void calculateFieldSize()
		{
			Int32 minSize	= windowHeight < windowWidth ? windowHeight : windowWidth;
			fieldSize		= minSize / 10;
			boardSize		= 8 * fieldSize;
			panelSize		= 6 * ( fieldSize + 15 );

			if ( mChessBoardCollection != null )
			{
				foreach ( BoardItem item in mChessBoardCollection )
				{
					item.fieldSize = fieldSize;
				}
			}

			if ( mBlackFigureCollection != null )
			{
				foreach ( BoardItem item in mBlackFigureCollection )
				{
					item.fieldSize = fieldSize;
				}
			}
			if ( mWhiteFigureCollection != null )
			{
				foreach ( BoardItem item in mWhiteFigureCollection )
				{
					item.fieldSize = fieldSize;
				}
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public String windowState
		{
			get
			{
				return mWindowState;
			}
			set
			{
				if ( mWindowState != value )
				{
					mWindowState = value;
					if ( mWindowState == "Maximized" )
					{
						windowWidth		= ( Int32 )SystemParameters.WorkArea.Width;
						windowHeight	= ( Int32 )SystemParameters.WorkArea.Height;
					}
					calculateFieldSize();
					OnPropertyChanged( "windowState" );
				}
			}
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------

		public Int32 windowWidth
		{
			get
			{
				return mWindowWidth;
			}
			set
			{
				if ( mWindowWidth != value )
				{
					mWindowWidth = value;
					calculateFieldSize();
					OnPropertyChanged( "windowWidth" );
				}
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public Int32 windowHeight
		{
			get
			{
				return mWindowHeight;
			}
			set
			{
				if ( mWindowHeight != value )
				{
					mWindowHeight = value;
					calculateFieldSize();
					OnPropertyChanged( "windowHeight" );
				}
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public Int32 fieldSize
		{
			get
			{
				return mFieldSize;
			}
			set
			{
				if ( mFieldSize != value )
				{
					mFieldSize = value;
					OnPropertyChanged( "fieldSize" );
				}
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public Int32 boardSize
		{
			get
			{
				return mBoardSize;
			}
			set
			{
				if ( mBoardSize != value )
				{
					mBoardSize = value;
					OnPropertyChanged( "boardSize" );
				}
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------
		public Int32 panelSize
		{
			get
			{
				return mPanelSize;
			}
			set
			{
				if ( mPanelSize != value )
				{
					mPanelSize = value;
					OnPropertyChanged( "panelSize" );
				}
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public ChessBoardModel chessBoardModel
		{
			get
			{
				return mChessBoardModel;
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public Boolean									isStartBtnClicked { get; set; }
		public ObservableCollection< BoardItem >		mChessBoardCollection { get; set; }
		public ObservableCollection< BoardItem >		mWhiteFigureCollection { get; set; }
		public ObservableCollection< BoardItem >		mBlackFigureCollection { get; set; }

		public DelegateCommand							startBtnClicked { get; set; }
		public DelegateCommand							cancelBtnClicked { get; set; }
		public DelegateCommand							deleteSelectedClicked { get; set; }

		private FigureItem								selectedPanelItem { get; set; }

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public event EventHandler						closeCustomBoardView;

		private Int32									mLastClickedField;

		private ChessBoardModel							mChessBoardModel;

		private String									mWindowState;
		private Int32									mWindowWidth;
		private Int32									mWindowHeight;

		private Int32									mFieldSize;
		private Int32									mBoardSize;
		private Int32									mPanelSize;
	}
}
