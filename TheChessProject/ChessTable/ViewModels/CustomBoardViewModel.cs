using System;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using ChessTable.ViewModels.ImplementedInterfaces;
using ChessTable.Common;
using ChessTable.Model;
using System.Collections.Generic;

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
			saveClicked				= new DelegateCommand( X => onSaveClicked() );
			loadClicked				= new DelegateCommand( X => onLoadClicked() );

			mChessBoardCollection	= new ObservableCollection< BoardItem >();
			mBlackFigureCollection	= new ObservableCollection< BoardItem >();
			mWhiteFigureCollection	= new ObservableCollection< BoardItem >();
			savedPositions			= new ObservableCollection< String >();
			mChessBoardModel		= new ChessBoardModel( aPlayer1Color, aStartingColor, aPlayer1Algorithm, aPlayer2Algorithm );

			mTablePositions			= new TablePositions();

			setSavedPositions();
			setupCustomBoard();
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		private void onStartBtnClicked()
		{
			Boolean isKingOn = false;
			foreach( var item in mChessBoardModel.whiteFigures )
			{
				if ( item.figureItem.figureType == FigureType.MOVED_KING )
				{
					isKingOn = true;
					break;
				}
			}
			if ( ! isKingOn )
			{
				MessageBox.Show( "White King is missing!" );
				return;
			}

			isKingOn = false;
			foreach( var item in mChessBoardModel.blackFigures )
			{
				if ( item.figureItem.figureType == FigureType.MOVED_KING )
				{
					isKingOn = true;
					break;
				}
			}
			if ( ! isKingOn )
			{
				MessageBox.Show( "Black King is missing!" );
				return;
			}

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

		void onSaveClicked()
		{
			View.Save save = new View.Save();
			if ( save.ShowDialog() == false )
			{
				return;
			}

			String saveName = save.SaveName;
			Boolean isFiguresOnTable = false;
			String saveValue = getTablePosition( ref isFiguresOnTable );
			if ( isFiguresOnTable )
			{
				mTablePositions.addNewPosition( saveName, saveValue );
				savedPositions.Add( saveName );
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		void onLoadClicked()
		{
			String selectedName = selectedPosition;
			foreach ( Positions pos in mTablePositions.tablePositions )
			{
				if ( pos.name == selectedName )
				{
					refreshTable( pos.value );
					return;
				}
			}
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

			mBlackFigureCollection[ 0 ].figureItem = new FigureItem( Colors.BLACK ,FigureType.MOVED_KING	);
			mBlackFigureCollection[ 1 ].figureItem = new FigureItem( Colors.BLACK, FigureType.QUEEN			);
			mBlackFigureCollection[ 2 ].figureItem = new FigureItem( Colors.BLACK, FigureType.MOVED_ROOK	);
			mBlackFigureCollection[ 3 ].figureItem = new FigureItem( Colors.BLACK, FigureType.BISHOP		);
			mBlackFigureCollection[ 4 ].figureItem = new FigureItem( Colors.BLACK, FigureType.KNIGHT		);
			mBlackFigureCollection[ 5 ].figureItem = new FigureItem( Colors.BLACK, FigureType.PAWN			);

			mWhiteFigureCollection[ 0 ].figureItem = new FigureItem( Colors.WHITE, FigureType.MOVED_KING	);
			mWhiteFigureCollection[ 1 ].figureItem = new FigureItem( Colors.WHITE, FigureType.QUEEN			);
			mWhiteFigureCollection[ 2 ].figureItem = new FigureItem( Colors.WHITE, FigureType.MOVED_ROOK	);
			mWhiteFigureCollection[ 3 ].figureItem = new FigureItem( Colors.WHITE, FigureType.BISHOP		);
			mWhiteFigureCollection[ 4 ].figureItem = new FigureItem( Colors.WHITE, FigureType.KNIGHT		);
			mWhiteFigureCollection[ 5 ].figureItem = new FigureItem( Colors.WHITE, FigureType.PAWN			);
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
				if ( aArguments.figureItem.color == selectedPanelItem.color && aArguments.figureItem.figureType == selectedPanelItem.figureType ) //Same Figure?
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
							mChessBoardModel.whiteFigures.Where( X => X.index == mLastClickedField ).FirstOrDefault().figureItem = new FigureItem( selectedPanelItem );
							mChessBoardCollection[ aArguments.index ].figureItem = new FigureItem( selectedPanelItem );
						}
						else // Is That Black?
						{
							mChessBoardModel.blackFigures.Where( X => X.index == mLastClickedField ).FirstOrDefault().figureItem = new FigureItem( selectedPanelItem );
							mChessBoardCollection[ aArguments.index ].figureItem = new FigureItem( selectedPanelItem );
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
								figureItem	= new FigureItem( selectedPanelItem ),
								x			= aArguments.x,
								y			= aArguments.y,
								index		= aArguments.index
							} );
							mChessBoardCollection[ aArguments.index ].figureItem = new FigureItem( selectedPanelItem );
						}
						else // is that Black?
						{
							ModelItem oldItem = mChessBoardModel.whiteFigures.Where( X => X.index == mLastClickedField ).FirstOrDefault();
							mChessBoardModel.whiteFigures.Remove( oldItem );

							mChessBoardModel.blackFigures.Add( new ModelItem
							{
								figureItem	= new FigureItem( selectedPanelItem ),
								x			= aArguments.x,
								y			= aArguments.y,
								index		= aArguments.index
							} );
							mChessBoardCollection[ aArguments.index ].figureItem = new FigureItem( selectedPanelItem );
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

			if ( aArguments.figureItem.color == selectedPanelItem.color && aArguments.figureItem.figureType == selectedPanelItem.figureType )
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
						figureItem	= new FigureItem( selectedPanelItem ),
						x			= aArguments.x,
						y			= aArguments.y,
						index		= aArguments.index
					} );
				}
				else
				{
					mChessBoardModel.blackFigures.Add( new ModelItem
					{
						figureItem	= new FigureItem( selectedPanelItem ),
						x			= aArguments.x,
						y			= aArguments.y,
						index		= aArguments.index
					} );
				}
				mChessBoardCollection[ aArguments.index ].highlightColor	= Colors.RED;
				mChessBoardCollection[ aArguments.index ].figureItem		= new FigureItem( selectedPanelItem );
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
						figureItem	= new FigureItem( selectedPanelItem ),
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
						figureItem	= new FigureItem( selectedPanelItem ),
						x			= aArguments.x,
						y			= aArguments.y,
						index		= aArguments.index
					} );
				}
				mChessBoardCollection[ aArguments.index ].highlightColor	= Colors.RED;
				mChessBoardCollection[ aArguments.index ].figureItem		= new FigureItem( selectedPanelItem );
				return;
			}

			if ( aArguments.figureItem.color == Colors.WHITE )
			{
				mChessBoardModel.whiteFigures.Where( X => X.index == mLastClickedField ).FirstOrDefault().figureItem = new FigureItem( selectedPanelItem );
			}
			else
			{
				mChessBoardModel.blackFigures.Where( X => X.index == mLastClickedField ).FirstOrDefault().figureItem = new FigureItem( selectedPanelItem );
			}
			mChessBoardCollection[ aArguments.index ].highlightColor	= Colors.RED;
			mChessBoardCollection[ aArguments.index ].figureItem		= new FigureItem( selectedPanelItem );
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		private void onPanelClicked( Object aSender, FieldClickedEventArg aArguments )
		{
			if ( selectedPanelItem.color == aArguments.figureItem.color && selectedPanelItem.figureType == aArguments.figureItem.figureType  )
			{
				mBlackFigureCollection.Concat( mWhiteFigureCollection ).Where( X => X.figureItem.color == selectedPanelItem.color && X.figureItem.figureType == selectedPanelItem.figureType ).FirstOrDefault().highlightColor = Colors.NO_COLOR;
				selectedPanelItem = new FigureItem( Colors.NO_COLOR, FigureType.NO_FIGURE );
				return;
			}
			if ( selectedPanelItem.figureType != FigureType.NO_FIGURE )
			{
				mBlackFigureCollection.Concat( mWhiteFigureCollection ).Where( X => X.figureItem.color == selectedPanelItem.color && X.figureItem.figureType == selectedPanelItem.figureType ).FirstOrDefault().highlightColor = Colors.NO_COLOR;
			}

			selectedPanelItem = new FigureItem( aArguments.figureItem );

			mBlackFigureCollection.Concat( mWhiteFigureCollection ).Where( X => X.figureItem.color == selectedPanelItem.color && X.figureItem.figureType == selectedPanelItem.figureType ).FirstOrDefault().highlightColor = Colors.RED;
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

		public String selectedPosition
		{
			get
			{
				return mSelectedPosition;
			}
			set
			{
				if ( value != mSelectedPosition )
				{
					mSelectedPosition = value;
					OnPropertyChanged( "selectedPosition" );
				}
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public String getTablePosition( ref Boolean isFigureOnTable )
		{
			String returnString = "";
			isFigureOnTable = false;
			foreach( BoardItem item in mChessBoardCollection )
			{
				String tempString = getFigureId( item.figureItem );
				if ( tempString != "0" )
				{
					isFigureOnTable = true;
				}
				returnString = returnString + tempString + ",";
			}

			return returnString;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public String getFigureId( FigureItem aItem )
		{
			String returnString;

			switch ( aItem.figureType )
			{
			case FigureType.PAWN:
				{
					returnString = "1";
				} break;
			case FigureType.EN_PASSANT_PAWN:
				{
					returnString = "2";
				} break;
			case FigureType.KNIGHT:
				{
					returnString = "3";
				} break;
			case FigureType.BISHOP:
				{
					returnString = "4";
				} break;
			case FigureType.ROOK:
				{
					returnString = "5";
				} break;
			case FigureType.MOVED_ROOK:
				{
					returnString = "6";
				} break;
			case FigureType.QUEEN:
				{
					returnString = "9";
				} break;
			case FigureType.KING:
				{
					returnString = "10";
				} break;
			case FigureType.MOVED_KING:
				{
					returnString = "11";
				} break;
			default:
				{
					returnString = "0";
				} break;
			}

			if ( aItem.color == Colors.BLACK )
			{
				returnString = "-" + returnString;
			}
			return returnString;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------
		void setSavedPositions()
		{
			savedPositions.Clear();
			List< String > saveNames = mTablePositions.saveNames;

			foreach ( String saveName in saveNames )
			{
				savedPositions.Add( saveName );
			}

			if ( savedPositions.Count != 0 )
			{
				selectedPosition = savedPositions.First();
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		void refreshTable( String aValue )
		{
			List< String > list = aValue.Split( ',' ).ToList();
			mChessBoardModel.whiteFigures.Clear();
			mChessBoardModel.blackFigures.Clear();

			Int32 index = 0;
			foreach ( BoardItem item in mChessBoardCollection )
			{
				item.figureItem = getFigureItemFromPosition( list[ index ] );

				if ( item.figureItem.color == Colors.BLACK )
				{
					mChessBoardModel.blackFigures.Add( new ModelItem( item.X, item.Y, Colors.BLACK, item.figureItem.figureType ) );
				}
				if ( item.figureItem.color == Colors.WHITE )
				{
					mChessBoardModel.whiteFigures.Add( new ModelItem( item.X, item.Y, Colors.WHITE, item.figureItem.figureType ) );
				}

				index++;
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		protected FigureItem getFigureItemFromPosition( String aFigureItem )
		{
			Int32 figureValue = Int32.Parse( aFigureItem );
			switch ( figureValue )
			{
			case 1:		return new FigureItem( Colors.WHITE,	FigureType.PAWN				);
			case 2:		return new FigureItem( Colors.WHITE,	FigureType.EN_PASSANT_PAWN	);
			case 3:		return new FigureItem( Colors.WHITE,	FigureType.KNIGHT			);
			case 4:		return new FigureItem( Colors.WHITE,	FigureType.BISHOP			);
			case 5:		return new FigureItem( Colors.WHITE,	FigureType.ROOK				);
			case 6:		return new FigureItem( Colors.WHITE,	FigureType.MOVED_ROOK		);
			case 9:		return new FigureItem( Colors.WHITE,	FigureType.QUEEN			);
			case 10:	return new FigureItem( Colors.WHITE,	FigureType.KING				);
			case 11:	return new FigureItem( Colors.WHITE,	FigureType.MOVED_KING		);

			case -1:	return new FigureItem( Colors.BLACK,	FigureType.PAWN				);
			case -2:	return new FigureItem( Colors.BLACK,	FigureType.EN_PASSANT_PAWN	);
			case -3:	return new FigureItem( Colors.BLACK,	FigureType.KNIGHT			);
			case -4:	return new FigureItem( Colors.BLACK,	FigureType.BISHOP			);
			case -5:	return new FigureItem( Colors.BLACK,	FigureType.ROOK				);
			case -6:	return new FigureItem( Colors.BLACK,	FigureType.MOVED_ROOK		);
			case -9:	return new FigureItem( Colors.BLACK,	FigureType.QUEEN			);
			case -10:	return new FigureItem( Colors.BLACK,	FigureType.KING				);
			case -11:	return new FigureItem( Colors.BLACK,	FigureType.MOVED_KING		);

			default:	return new FigureItem( Colors.NO_COLOR,	FigureType.NO_FIGURE		);
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public Boolean									isStartBtnClicked { get; set; }
		public ObservableCollection< BoardItem >		mChessBoardCollection { get; set; }
		public ObservableCollection< BoardItem >		mWhiteFigureCollection { get; set; }
		public ObservableCollection< BoardItem >		mBlackFigureCollection { get; set; }
		public ObservableCollection< String >			savedPositions { get; set; }

		public DelegateCommand							startBtnClicked { get; set; }
		public DelegateCommand							cancelBtnClicked { get; set; }
		public DelegateCommand							deleteSelectedClicked { get; set; }
		public DelegateCommand							saveClicked { get; set; }
		public DelegateCommand							loadClicked { get; set; }

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
		private String									mSelectedPosition;
		private TablePositions							mTablePositions;
	}
}
