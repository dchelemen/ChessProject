using System;
using ChessTable.Common;
using ChessTable.ViewModels.ImplementedInterfaces;

namespace ChessTable.ViewModels
{
	public class BoardItem : ViewModelBase
	{
		public BoardItem()
		{
			onFieldClickedCommand = new DelegateCommand( x => onFieldClicked() );
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public void onFieldClicked()
		{
			fieldClicked( this, new FieldClickedEventArg
			{
				x			= X,
				y			= Y,
				index		= Index,
				figureItem	= figureItem
			} );
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public Colors fieldColor
		{
			get
			{
				return mFieldColor;
			}
			set
			{
				if ( mFieldColor != value )
				{
					mFieldColor = value;
					OnPropertyChanged( "fieldColor" );
				}
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public Colors highlightColor
		{
			get
			{
				return mHighlightColor;
			}
			set
			{
				if ( mHighlightColor != value )
				{
					mHighlightColor = value;
					OnPropertyChanged( "highlightColor" );
				}
			}
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public FigureItem figureItem
		{
			get
			{
				return mFigureItem;
			}
			set
			{
				if ( mFigureItem != value )
				{
					mFigureItem = value;
					OnPropertyChanged( "figureItem" );
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

		public DelegateCommand								onFieldClickedCommand { get; private set; }
		public Int32										X { get; set; }

		public Int32										Y { get; set; }

		public Int32										Index { get; set; }

		public Colors										Color { get; set; }

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public event EventHandler< FieldClickedEventArg >	fieldClicked;

		private Colors										mFieldColor;
		private Colors										mHighlightColor;

		private FigureItem									mFigureItem;
		private Int32										mFieldSize;
	}
}
