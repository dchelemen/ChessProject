using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
                index = Index
            } );
        }


        public String fieldColor
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
        public String borderColor
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
        public String fieldFigure
        {
            get
            {
                return mFieldFigure;
            }
            set
            {
                if ( mFieldFigure != value )
                {
                    mFieldFigure = value;
                    OnPropertyChanged( "fieldFigure" );
                }
            }
        }
        public FigureType figureType
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

        public event EventHandler< FieldClickedEventArg > fieldClicked;

        private String mFieldColor;
        private String mBorderColor;
        private String mFieldFigure;
        private FigureType mFigureType;
        private Int32 mFieldSize;
    }
}
