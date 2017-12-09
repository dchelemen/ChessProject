using System;
using ChessTable.View;
using ChessTable.ViewModels.ImplementedInterfaces;
using System.Collections.ObjectModel;
using ChessTable.Model;
using ChessTable.Common;

namespace ChessTable.ViewModels
{
	public class MenuViewModel : ViewModelBase
	{ 
		public MenuViewModel()
		{
			mGameTypes						= new ObservableCollection< String >();
			mColors							= new ObservableCollection< String >();
			mPlayers						= new ObservableCollection< String >();
			mPlayerAlgorithms				= new ObservableCollection< String >();

			onMenuStartBtnClickedCommand	= new DelegateCommand( x => onMenuStartBtnClicked() );
			setupComboBoxes();
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public void onMenuStartBtnClicked()
		{
			Boolean isGameReady = false;
			switch ( mSelectedGameType )
			{
			case GameType.CUSTOM_GAME:
				{
					CustomBoardViewModel customBoardViewModel	= new CustomBoardViewModel( selectedColor, selectedStartingColor, playerOneAlgorithm, playerTwoAlgorithm );
					customBoardViewModel.closeCustomBoardView	+= new EventHandler( onCloseCustomBoardView );
					mCustomBoardView				= new CustomBoardView();
					mCustomBoardView.DataContext	= customBoardViewModel;
					mCustomBoardView.ShowDialog();
					mChessBoardModel				= customBoardViewModel.chessBoardModel;
					isGameReady						= customBoardViewModel.isStartBtnClicked;
				} break;
			case GameType.STANDARD_GAME:
				{
					isGameReady = standardGameSetup();
				}
				break;
			}

			if ( ! isGameReady )
			{
				return;
			}

			mChessBoardView					= new ChessBoardView();
			mChessBoardViewModel			= new ChessBoardViewModel( mChessBoardModel );
			mChessBoardView.DataContext		= mChessBoardViewModel;
			mChessBoardView.ShowDialog();
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		private Boolean standardGameSetup()
		{
			mChessBoardModel = new ChessBoardModel( selectedColor, selectedStartingColor, playerOneAlgorithm, playerTwoAlgorithm );

			if ( selectedColor == Colors.WHITE )
			{
				for ( Int32 column = 0; column < 8; column++ ) // set Pawns
				{
					mChessBoardModel.whiteFigures.Add( new ModelItem
					{
						index		= ( 6 * 8 ) + column,
						x			= 6,
						y			= column,
						figureItem	= new FigureItem( Colors.WHITE, FigureType.PAWN )
					} );

					mChessBoardModel.blackFigures.Add( new ModelItem
					{
						index		= ( 1 * 8 ) + column,
						x			= 1,
						y			= column,
						figureItem	= new FigureItem( Colors.BLACK, FigureType.PAWN )
					} );
				}

				mChessBoardModel.whiteFigures.Add( new ModelItem
				{
					index		= ( 7 * 8 ) + 0,
					x			= 7,
					y			= 0,
					figureItem	= new FigureItem( Colors.WHITE, FigureType.ROOK )
				} );

				mChessBoardModel.whiteFigures.Add( new ModelItem
				{
					index		= ( 7 * 8 ) + 7,
					x			= 7,
					y			= 7,
					figureItem	= new FigureItem( Colors.WHITE, FigureType.ROOK )
				} );

				mChessBoardModel.whiteFigures.Add( new ModelItem
				{
					index		= ( 7 * 8 ) + 1,
					x			= 7,
					y			= 1,
					figureItem	= new FigureItem( Colors.WHITE, FigureType.KNIGHT )
				} );
				mChessBoardModel.whiteFigures.Add( new ModelItem
				{
					index		= ( 7 * 8 ) + 6,
					x			= 7,
					y			= 6,
					figureItem	= new FigureItem( Colors.WHITE, FigureType.KNIGHT )
				} );
				mChessBoardModel.whiteFigures.Add( new ModelItem
				{
					index		= ( 7 * 8 ) + 2,
					x			= 7,
					y			= 2,
					figureItem	= new FigureItem( Colors.WHITE, FigureType.BISHOP )
				} );
				mChessBoardModel.whiteFigures.Add( new ModelItem
				{
					index		= ( 7 * 8 ) + 5,
					x			= 7,
					y			= 5,
					figureItem	= new FigureItem( Colors.WHITE, FigureType.BISHOP )
				} );
				mChessBoardModel.whiteFigures.Add( new ModelItem
				{
					index		= ( 7 * 8 ) + 3,
					x			= 7,
					y			= 3,
					figureItem	= new FigureItem( Colors.WHITE, FigureType.QUEEN )
				} );
				mChessBoardModel.whiteFigures.Add( new ModelItem
				{
					index		= ( 7 * 8 ) + 4,
					x			= 7,
					y			= 4,
					figureItem	= new FigureItem( Colors.WHITE, FigureType.KING )
				} );

				mChessBoardModel.blackFigures.Add( new ModelItem
				{
					index		= ( 0 * 8 ) + 0,
					x			= 0,
					y			= 0,
					figureItem	= new FigureItem( Colors.BLACK, FigureType.ROOK )
				} );
				mChessBoardModel.blackFigures.Add( new ModelItem
				{
					index		= ( 0 * 8 ) + 7,
					x			= 0,
					y			= 7,
					figureItem	= new FigureItem( Colors.BLACK, FigureType.ROOK )
				} );
				mChessBoardModel.blackFigures.Add( new ModelItem
				{
					index		= ( 0 * 8 ) + 1,
					x			= 0,
					y			= 1,
					figureItem	= new FigureItem( Colors.BLACK, FigureType.KNIGHT )
				} );
				mChessBoardModel.blackFigures.Add( new ModelItem
				{
					index		= ( 0 * 8 ) + 6,
					x			= 0,
					y			= 6,
					figureItem	= new FigureItem( Colors.BLACK, FigureType.KNIGHT )
				} );
				mChessBoardModel.blackFigures.Add( new ModelItem
				{
					index		= ( 0 * 8 ) + 2,
					x			= 0,
					y			= 2,
					figureItem	= new FigureItem( Colors.BLACK, FigureType.BISHOP )
				} );
				mChessBoardModel.blackFigures.Add( new ModelItem
				{
					index		= ( 0 * 8 ) + 5,
					x			= 0,
					y			= 5,
					figureItem	= new FigureItem( Colors.BLACK, FigureType.BISHOP )
				} );
				mChessBoardModel.blackFigures.Add( new ModelItem
				{
					index		= ( 0 * 8 ) + 3,
					x			= 0,
					y			= 3,
					figureItem	= new FigureItem( Colors.BLACK, FigureType.QUEEN )
				} );
				mChessBoardModel.blackFigures.Add( new ModelItem
				{
					index		= ( 0 * 8 ) + 4,
					x			= 0,
					y			= 4,
					figureItem	= new FigureItem( Colors.BLACK, FigureType.KING )
				} );
			}
			else
			{
				for ( Int32 column = 0; column < 8; column++ )
				{
					mChessBoardModel.blackFigures.Add( new ModelItem
					{
						index		= ( 6 * 8 ) + column,
						x			= 6,
						y			= column,
						figureItem	= new FigureItem( Colors.BLACK, FigureType.PAWN )
					} );

					mChessBoardModel.whiteFigures.Add( new ModelItem
					{
						index		= ( 1 * 8 ) + column,
						x			= 1,
						y			= column,
						figureItem	= new FigureItem( Colors.WHITE, FigureType.PAWN )
					} );
				}

				mChessBoardModel.blackFigures.Add( new ModelItem
				{
					index		= ( 7 * 8 ) + 0,
					x			= 7,
					y			= 0,
					figureItem	= new FigureItem( Colors.BLACK, FigureType.ROOK )
				} );

				mChessBoardModel.blackFigures.Add( new ModelItem
				{
					index		= ( 7 * 8 ) + 7,
					x			= 7,
					y			= 7,
					figureItem	= new FigureItem( Colors.BLACK, FigureType.ROOK )
				} );

				mChessBoardModel.blackFigures.Add( new ModelItem
				{
					index		= ( 7 * 8 ) + 1,
					x			= 7,
					y			= 1,
					figureItem	= new FigureItem( Colors.BLACK, FigureType.KNIGHT )
				} );
				mChessBoardModel.blackFigures.Add( new ModelItem
				{
					index		= ( 7 * 8 ) + 6,
					x			= 7,
					y			= 6,
					figureItem	= new FigureItem( Colors.BLACK, FigureType.KNIGHT )
				} );
				mChessBoardModel.blackFigures.Add( new ModelItem
				{
					index		= ( 7 * 8 ) + 2,
					x			= 7,
					y			= 2,
					figureItem	= new FigureItem( Colors.BLACK, FigureType.BISHOP )
				} );
				mChessBoardModel.blackFigures.Add( new ModelItem
				{
					index		= ( 7 * 8 ) + 5,
					x			= 7,
					y			= 5,
					figureItem	= new FigureItem( Colors.BLACK, FigureType.BISHOP )
				} );
				mChessBoardModel.blackFigures.Add( new ModelItem
				{
					index		= ( 7 * 8 ) + 3,
					x			= 7,
					y			= 3,
					figureItem	= new FigureItem( Colors.BLACK, FigureType.KING )
				} );
				mChessBoardModel.blackFigures.Add( new ModelItem
				{
					index		= ( 7 * 8 ) + 4,
					x			= 7,
					y			= 4,
					figureItem	= new FigureItem( Colors.BLACK, FigureType.QUEEN )
				} );

				mChessBoardModel.whiteFigures.Add( new ModelItem
				{
					index		= ( 0 * 8 ) + 0,
					x			= 0,
					y			= 0,
					figureItem	= new FigureItem( Colors.WHITE, FigureType.ROOK )
				} );
				mChessBoardModel.whiteFigures.Add( new ModelItem
				{
					index		= ( 0 * 8 ) + 7,
					x			= 0,
					y			= 7,
					figureItem	= new FigureItem( Colors.WHITE, FigureType.ROOK )
				} );
				mChessBoardModel.whiteFigures.Add( new ModelItem
				{
					index		= ( 0 * 8 ) + 1,
					x			= 0,
					y			= 1,
					figureItem	= new FigureItem( Colors.WHITE, FigureType.KNIGHT )
				} );
				mChessBoardModel.whiteFigures.Add( new ModelItem
				{
					index		= ( 0 * 8 ) + 6,
					x			= 0,
					y			= 6,
					figureItem	= new FigureItem( Colors.WHITE, FigureType.KNIGHT )
				} );
				mChessBoardModel.whiteFigures.Add( new ModelItem
				{
					index		= ( 0 * 8 ) + 2,
					x			= 0,
					y			= 2,
					figureItem	= new FigureItem( Colors.WHITE, FigureType.BISHOP )
				} );
				mChessBoardModel.whiteFigures.Add( new ModelItem
				{
					index		= ( 0 * 8 ) + 5,
					x			= 0,
					y			= 5,
					figureItem	= new FigureItem( Colors.WHITE, FigureType.BISHOP )
				} );
				mChessBoardModel.whiteFigures.Add( new ModelItem
				{
					index		= ( 0 * 8 ) + 3,
					x			= 0,
					y			= 3,
					figureItem	= new FigureItem( Colors.WHITE, FigureType.KING )
				} );
				mChessBoardModel.whiteFigures.Add( new ModelItem
				{
					index		= ( 0 * 8 ) + 4,
					x			= 0,
					y			= 4,
					figureItem	= new FigureItem( Colors.WHITE, FigureType.QUEEN )
				} );
			}

			return true;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		private void onCloseCustomBoardView( Object sender, EventArgs aEventArgs )
		{
			mCustomBoardView.Close();
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		private void setupComboBoxes()
		{
			mGameTypes.Clear();
			mGameTypes.Add( "Standard Game"	);
			mGameTypes.Add( "Custom Game"	);

			selectedGameType = GameType.STANDARD_GAME;

			mColors.Clear();
			mColors.Add( "White" );
			mColors.Add( "Black" );

			selectedColor = Colors.WHITE;

			mPlayers.Clear();
			mPlayers.Add( "White" );
			mPlayers.Add( "Black" );

			selectedStartingColor = Colors.WHITE;

			mPlayerAlgorithms.Clear();
			mPlayerAlgorithms.Add( "Human"							);
			mPlayerAlgorithms.Add( "Random"							);
			mPlayerAlgorithms.Add( "Alpha-Beta"						);
			mPlayerAlgorithms.Add( "Alpha-Beta Random"				);
			mPlayerAlgorithms.Add( "Alpha-Beta with weight"			);
			mPlayerAlgorithms.Add( "Alpha-Beta Random with weight"	);

			playerOneAlgorithm = Algorithm.HUMAN;
			playerTwoAlgorithm = Algorithm.HUMAN;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public GameType selectedGameType
		{
			get
			{
				return mSelectedGameType;
			}
			set
			{
				if ( value != mSelectedGameType )
				{
					mSelectedGameType = value;
					OnPropertyChanged( "selectedGameType" );
				}
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public Colors selectedColor
		{
			get
			{
				return mSelectedColor;
			}
			set
			{
				if ( value != mSelectedColor )
				{
					mSelectedColor = value;
					OnPropertyChanged( "selectedColor" );
				}
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public Colors selectedStartingColor
		{
			get
			{
				return mSelectedStartingColor;
			}
			set
			{
				if ( value != mSelectedStartingColor )
				{
					mSelectedStartingColor = value;
					OnPropertyChanged( "selectedPlayer" );
				}
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public Algorithm playerOneAlgorithm
		{
			get
			{
				return mPlayerOneAlgorithm;
			}
			set
			{
				if ( value != mPlayerOneAlgorithm )
				{
					mPlayerOneAlgorithm = value;
					OnPropertyChanged( "selectedPlayerOneAlgorithm" );
				}
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public Algorithm playerTwoAlgorithm
		{
			get
			{
				return mPlayerTwoAlgorithm;
			}
			set
			{
				if ( value != mPlayerTwoAlgorithm )
				{
					mPlayerTwoAlgorithm = value;
					OnPropertyChanged( "selectedPlayerTwoAlgorithm" );
				}
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public DelegateCommand								onMenuStartBtnClickedCommand { get; private set; }

		public ObservableCollection< String >				mGameTypes { get; set; }
		private GameType mSelectedGameType;
		public ObservableCollection< String >				mColors { get; set; }
		private Colors mSelectedColor;
		public ObservableCollection< String >				mPlayers { get; set; }
		private Colors mSelectedStartingColor;
		public ObservableCollection< String >				mPlayerAlgorithms { get; set; }

		//-----------------------------------------------------------------------------------------------------------------------------------------

		private Algorithm									mPlayerOneAlgorithm;
		private Algorithm									mPlayerTwoAlgorithm;

		private CustomBoardView								mCustomBoardView;

		private ChessBoardView								mChessBoardView;
		private ChessBoardViewModel							mChessBoardViewModel;

		private ChessBoardModel								mChessBoardModel;
	}
}
