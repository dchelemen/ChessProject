using System;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using ChessTable.ViewModels.ImplementedInterfaces;
using ChessTable.Common;
using ChessTable.Model;

namespace ChessTable.ViewModels
{
	public class ChooseFigureViewModel : ViewModelBase 
	{
		//-----------------------------------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// Constructor for CustomBoardViewModel
		/// </summary>
		public ChooseFigureViewModel()
		{
			windowState				= "Normal";
			windowWidth				= 200;
			windowHeight			= 60;
			fieldSize				= 48;

			selectedFigureType		= FigureType.NO_FIGURE;

			okBtnClicked			= new DelegateCommand( X => onOkBtnClicked() );
			
			mFigureCollection		= new ObservableCollection< BoardItem >();

			setupChooseFigure();
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		private void onOkBtnClicked()
		{
			closeChooseFigureView( this, selectedFigureType );
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// creates the board colors (black and white)
		/// </summary>
		private void setupChooseFigure()
		{
			mFigureCollection.Clear();

			for( Int16 column = 0; column < 4; column++ )
			{
				mFigureCollection.Add( new BoardItem
				{
					X					= 0,
					Y					= column, //its for Black
					Index				= column,
					fieldColor			= Colors.BLACK,
					fieldSize			= 48,
					highlightColor		= Colors.NO_COLOR,
					Color				= Colors.BLACK
				} );

				mFigureCollection[ column ].fieldClicked += new EventHandler< FieldClickedEventArg >( onPanelClicked );
			}

			mFigureCollection[ 0 ].figureItem = new FigureItem( Colors.WHITE ,FigureType.KNIGHT		);
			mFigureCollection[ 1 ].figureItem = new FigureItem( Colors.WHITE, FigureType.BISHOP		);
			mFigureCollection[ 2 ].figureItem = new FigureItem( Colors.WHITE, FigureType.ROOK		);
			mFigureCollection[ 3 ].figureItem = new FigureItem( Colors.WHITE, FigureType.QUEEN		);
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		private void onPanelClicked( Object aSender, FieldClickedEventArg aArguments )
		{
			if ( selectedFigureType != FigureType.NO_FIGURE )
			{
				mFigureCollection.Where( X => X.figureItem.figureType == selectedFigureType ).FirstOrDefault().highlightColor = Colors.NO_COLOR;
			}

			selectedFigureType = aArguments.figureItem.figureType;
			mFigureCollection.Where( X => X.figureItem.figureType == aArguments.figureItem.figureType ).FirstOrDefault().highlightColor = Colors.BLUE;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		private void calculateFieldSize()
		{
			Int32 minSize	= windowHeight < windowWidth ? windowHeight : windowWidth;
			fieldSize		= minSize / 10;
			panelSize		= 4 * ( fieldSize + 15 );

			if ( mFigureCollection != null )
			{
				foreach ( BoardItem item in mFigureCollection )
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
		
		public ObservableCollection< BoardItem >		mFigureCollection { get; set; }

		public DelegateCommand							okBtnClicked { get; set; }

		private FigureType								selectedFigureType { get; set; }

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public event EventHandler< FigureType >			closeChooseFigureView;

		private String									mWindowState;
		private Int32									mWindowWidth;
		private Int32									mWindowHeight;

		private Int32									mFieldSize;
		private Int32									mPanelSize;
	}
}
