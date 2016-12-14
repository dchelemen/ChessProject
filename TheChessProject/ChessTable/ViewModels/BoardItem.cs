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
        
        public void onFieldClicked()
        {
            fieldClicked( this, new FieldClickedEventArg
            {
                x = X,
                y = Y,
                index = Index,
                type = figureType
            } );
        }


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

        public Colors borderColor
        {
            get
            {
                return mBorderColor;
            }
            set
            {
                if ( mBorderColor != value )
                {
                    mBorderColor = value;
                    OnPropertyChanged( "borderColor" );
                }
            }
        }

        public Tuple<Colors, FigureType> figureType
        {
            get
            {
                return mFigureType;
            }
            set
            {
                if ( mFigureType != value )
                {
                    mFigureType = value;
                    OnPropertyChanged( "figureType" );
                }
            }
        }

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

        public DelegateCommand onFieldClickedCommand { get; private set; }
        public Int32 X { get; set; }

        public Int32 Y { get; set; }

        public Int32 Index { get; set; }

        public Colors Color { get; set; }

        public event EventHandler< FieldClickedEventArg > fieldClicked;

        private Colors mFieldColor;
        private Colors mBorderColor;

        private Tuple<Colors, FigureType> mFigureType;
        private Int32 mFieldSize;
    }
}
