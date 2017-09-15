using System;
using ChessTable.ViewModels.ImplementedInterfaces;
using System.Collections.ObjectModel;
using System.Windows;
using ChessTable.Common;
using ChessTable.Model;

namespace ChessTable.ViewModels
{
    class ChessBoardViewModel : ViewModelBase
    {
        //-----------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Constructor for CustomBoardViewModel
        /// </summary>
        public ChessBoardViewModel( ChessBoardModel aChessBoardModel )
        {
            mChessBoardModel = aChessBoardModel;
            mChessBoardModel.fieldClicked   += new EventHandler<PutFigureOnTheTableEventArg>( onPutFigureOnTheTableEventArg );
            mChessBoardModel.paintBorder    += new EventHandler<PaintBorderEventArg>( onPaintBorder );

            windowState             = "Normal";
            windowWidth             = 640;
            windowHeight            = 480;
            fieldSize               = 48;
            boardSize               = 384;
            mChessBoardCollection   = new ObservableCollection<BoardItem>();

            selectedPanelItem       = new Tuple<Colors, FigureType>( Colors.NO_COLOR, FigureType.NO_FIGURE );
            mLastClickedField       = -1;

            setupCustomBoard();
            mChessBoardModel.startModel();
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// creates the board colors (black and white)
        /// </summary>
        private void setupCustomBoard()
        {
            mChessBoardCollection.Clear();

            Colors color;
            Int32 index = 0;
            for ( Int16 row = 0; row < 8; row++ )
            {
                for ( Int16 column = 0; column < 8; column++ )
                {
                    color = ( row + column ) % 2 == 0 ? Colors.WHITE : Colors.BLACK;
                    mChessBoardCollection.Add( new BoardItem()
                    {
                        X           = row,
                        Y           = column,
                        Index       = index,
                        fieldColor  = color,
                        fieldSize   = 48,
                        figureType  = new Tuple<Colors, FigureType>( Colors.NO_COLOR, FigureType.NO_FIGURE ),
                        borderColor = color
                    } );
                    mChessBoardCollection[ index ].fieldClicked += new EventHandler<FieldClickedEventArg>( onFieldClicked );
                    index++;
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        private void onFieldClicked( Object aSender, FieldClickedEventArg aArguments )
        {
            mChessBoardModel.moveFigure( new ModelItem
            {
                index   = aArguments.index,
                type    = aArguments.type,
                x       = aArguments.x,
                y       = aArguments.y
            } );
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        private void onPutFigureOnTheTableEventArg( Object aSender, PutFigureOnTheTableEventArg aArguments )
        {
            mChessBoardCollection[ aArguments.index ].figureType = aArguments.type;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        private void onPaintBorder( Object aSender, PaintBorderEventArg aItemToPaint )
        {
            mChessBoardCollection[ aItemToPaint.index ].borderColor = aItemToPaint.color;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        private void calculateFieldSize()
        {
            Int32 minSize = windowHeight < windowWidth ? windowHeight : windowWidth;
            fieldSize = minSize / 10;
            boardSize = 8 * fieldSize;

            if ( mChessBoardCollection != null )
            {
                foreach ( BoardItem item in mChessBoardCollection )
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
                        windowWidth = ( Int32 )SystemParameters.WorkArea.Width;
                        windowHeight = ( Int32 )SystemParameters.WorkArea.Height;
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

        public ObservableCollection<BoardItem> mChessBoardCollection { get; set; }

        public Tuple<Colors, FigureType> selectedPanelItem { get; set; }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        private Int32 mLastClickedField;

        private String mWindowState;
        private Int32 mWindowWidth;
        private Int32 mWindowHeight;
         
        private Int32 mFieldSize;
        private Int32 mBoardSize;

        private ChessBoardModel mChessBoardModel;
    }
}
