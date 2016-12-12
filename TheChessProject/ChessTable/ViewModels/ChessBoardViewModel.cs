using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessTable.ViewModels.ImplementedInterfaces;
using System.Collections.ObjectModel;
using System.Windows;
using ChessTable.Common;

namespace ChessTable.ViewModels
{
    class ChessBoardViewModel : ViewModelBase
    {
        //-----------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Constructor for CustomBoardViewModel
        /// </summary>
        public ChessBoardViewModel()
        {
            windowState = "Normal";
            windowWidth = 640;
            windowHeight = 480;
            fieldSize = 48;
            boardSize = 384;
            mChessBoardCollection = new ObservableCollection<BoardItem>();
            mLastClicked = -1;

            selectedType = FigureType.NO_FIGURE;

            setupCustomBoard();
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        public string typesToString( FigureType aType )
        {
            switch ( aType )
            {
                case FigureType.BLACK_KING: return "/Images/Black_King.png";
                case FigureType.BLACK_QUEEN: return "/Images/Black_Queen.png";
                case FigureType.BLACK_ROOK: return "/Images/Black_Rook.png";
                case FigureType.BLACK_BISHOP: return "/Images/Black_Bishop.png";
                case FigureType.BLACK_KNIGHT: return "/Images/Black_Knight.png";
                case FigureType.BLACK_PAWN: return "/Images/Black_Pawn.png";
                case FigureType.WHITE_KING: return "/Images/White_King.png";
                case FigureType.WHITE_QUEEN: return "/Images/White_Queen.png";
                case FigureType.WHITE_ROOK: return "/Images/White_Rook.png";
                case FigureType.WHITE_BISHOP: return "/Images/White_Bishop.png";
                case FigureType.WHITE_KNIGHT: return "/Images/White_Knight.png";
                case FigureType.WHITE_PAWN: return "/Images/White_Pawn.png";
                case FigureType.NO_FIGURE: return null;
            }
            return null;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// creates the board colors (black and white)
        /// </summary>
        private void setupCustomBoard()
        {
            mChessBoardCollection.Clear();

            String color;
            Int32 index = 0;
            for ( Int16 row = 0; row < 8; row++ )
            {
                for ( Int16 column = 0; column < 8; column++ )
                {
                    color = ( row + column ) % 2 == 0 ? "White" : "Black";
                    mChessBoardCollection.Add( new BoardItem()
                    {
                        X = row,
                        Y = column,
                        Index = index,
                        fieldColor = color,
                        fieldFigure = "",
                        fieldSize = 48,
                        figureType = FigureType.NO_FIGURE,
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
            if ( selectedType == FigureType.NO_FIGURE )
            {
                return;
            }
            if ( mLastClicked != -1 )
            {
                mChessBoardCollection[ mLastClicked ].borderColor = ( mChessBoardCollection[ mLastClicked ].X + mChessBoardCollection[ mLastClicked ].Y ) % 2 == 0 ? "White" : "Black";
                mLastClicked = -1;
            }
            mChessBoardCollection[ aArguments.index ].fieldFigure = typesToString( selectedType );
            mChessBoardCollection[ aArguments.index ].borderColor = "Red";
            mLastClicked = aArguments.index;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        private FigureType getFigureType( Int32 aRow, Int32 aColor )
        {
            if ( aColor == 0 ) //thats the Black
            {
                switch ( aRow )
                {
                    case 0: return FigureType.BLACK_KING;
                    case 1: return FigureType.BLACK_QUEEN;
                    case 2: return FigureType.BLACK_ROOK;
                    case 3: return FigureType.BLACK_BISHOP;
                    case 4: return FigureType.BLACK_KNIGHT;
                    case 5: return FigureType.BLACK_PAWN;
                }
            }
            else
            {
                switch ( aRow )
                {
                    case 0: return FigureType.WHITE_KING;
                    case 1: return FigureType.WHITE_QUEEN;
                    case 2: return FigureType.WHITE_ROOK;
                    case 3: return FigureType.WHITE_BISHOP;
                    case 4: return FigureType.WHITE_KNIGHT;
                    case 5: return FigureType.WHITE_PAWN;
                }
            }

            return FigureType.NO_FIGURE;
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

        public FigureType selectedType { get; set; }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        private Int32 mLastClicked;

        private String mWindowState;
        private Int32 mWindowWidth;
        private Int32 mWindowHeight;

        private Int32 mFieldSize;
        private Int32 mBoardSize;
    }
}
