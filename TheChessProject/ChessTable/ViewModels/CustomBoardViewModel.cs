using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessTable.View;
using System.Windows;
using System.Collections.ObjectModel;
using ChessTable.ViewModels.ImplementedInterfaces;

namespace ChessTable.ViewModels
{
    public class CustomBoardViewModel : ViewModelBase 
    {
        //-----------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Constructor for CustomBoardViewModel
        /// </summary>
        public CustomBoardViewModel()
        {
            windowState     = "Normal";
            windowWidth     = 640;
            windowHeight    = 480;
            fieldSize       = 48;
            boardSize       = 384;
            selectedType    = FigureType.NO_FIGURE;
            mChessBoardCollection   = new ObservableCollection< BoardItem >();
            mBlackFigureCollection  = new ObservableCollection< BoardItem >();
            mWhiteFigureCollection  = new ObservableCollection< BoardItem >();
            mLastClicked = -1;
            mLastClickedPanel = new Tuple< int, int >( -1, -1 );

            setupCustomBoard();
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        public string typesToString( FigureType aType )
        {
            switch ( aType )
            {
                case FigureType.BLACK_KING:     return "/Images/Black_King.png";
                case FigureType.BLACK_QUEEN:    return "/Images/Black_Queen.png";
                case FigureType.BLACK_ROOK:     return "/Images/Black_Rook.png";
                case FigureType.BLACK_BISHOP:   return "/Images/Black_Bishop.png";
                case FigureType.BLACK_KNIGHT:   return "/Images/Black_Knight.png";
                case FigureType.BLACK_PAWN:     return "/Images/Black_Pawn.png";
                case FigureType.WHITE_KING:     return "/Images/White_King.png";
                case FigureType.WHITE_QUEEN:    return "/Images/White_Queen.png";
                case FigureType.WHITE_ROOK:     return "/Images/White_Rook.png";
                case FigureType.WHITE_BISHOP:   return "/Images/White_Bishop.png";
                case FigureType.WHITE_KNIGHT:   return "/Images/White_Knight.png";
                case FigureType.WHITE_PAWN:     return "/Images/White_Pawn.png";
                case FigureType.NO_FIGURE:      return null;
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
            mBlackFigureCollection.Clear();
            mWhiteFigureCollection.Clear();

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
                    mChessBoardCollection[ index ].fieldClicked += new EventHandler< EventArgs.FieldClickedEventArg >( onFieldClicked );
                    index++;
                }
            }

            for( Int16 row = 0; row < 6; row++ )
            {
                mBlackFigureCollection.Add( new BoardItem
                {
                    X = row,
                    Y = 0, //its for Black
                    Index = row,
                    fieldColor = "Black",
                    fieldSize = 48,
                    borderColor = "White"
                } );

                mWhiteFigureCollection.Add( new BoardItem
                {
                    X = row,
                    Y = 1, //its for White
                    Index = row,
                    fieldColor = "Black",
                    fieldSize = 48,
                    borderColor = "White"
                } );

                mBlackFigureCollection[ row ].fieldClicked += new EventHandler< EventArgs.FieldClickedEventArg >( onPanelClicked );
                mWhiteFigureCollection[ row ].fieldClicked += new EventHandler< EventArgs.FieldClickedEventArg >( onPanelClicked );
            }

            mBlackFigureCollection[ 0 ].fieldFigure = typesToString( FigureType.BLACK_KING      );
            mBlackFigureCollection[ 1 ].fieldFigure = typesToString( FigureType.BLACK_QUEEN     );
            mBlackFigureCollection[ 2 ].fieldFigure = typesToString( FigureType.BLACK_ROOK      );
            mBlackFigureCollection[ 3 ].fieldFigure = typesToString( FigureType.BLACK_BISHOP    );
            mBlackFigureCollection[ 4 ].fieldFigure = typesToString( FigureType.BLACK_KNIGHT    );
            mBlackFigureCollection[ 5 ].fieldFigure = typesToString( FigureType.BLACK_PAWN      );

            mWhiteFigureCollection[ 0 ].fieldFigure = typesToString( FigureType.WHITE_KING      );
            mWhiteFigureCollection[ 1 ].fieldFigure = typesToString( FigureType.WHITE_QUEEN     );
            mWhiteFigureCollection[ 2 ].fieldFigure = typesToString( FigureType.WHITE_ROOK      );
            mWhiteFigureCollection[ 3 ].fieldFigure = typesToString( FigureType.WHITE_BISHOP    );
            mWhiteFigureCollection[ 4 ].fieldFigure = typesToString( FigureType.WHITE_KNIGHT    );
            mWhiteFigureCollection[ 5 ].fieldFigure = typesToString( FigureType.WHITE_PAWN      );
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        private void onFieldClicked(Object aSender, EventArgs.FieldClickedEventArg aArguments )
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

        private void onPanelClicked( Object aSender, EventArgs.FieldClickedEventArg aArguments )
        {
            if ( mLastClickedPanel.Item1 == aArguments.index && mLastClickedPanel.Item2 == aArguments.y )
            {
                selectedType = FigureType.NO_FIGURE;
                if ( mLastClickedPanel.Item2 == 0 ) //this is the Black
                {
                    mBlackFigureCollection[ mLastClickedPanel.Item1 ].borderColor = "White";
                }
                else
                {
                    mWhiteFigureCollection[ mLastClickedPanel.Item1 ].borderColor = "White";
                }
                mLastClickedPanel = new Tuple<int, int>( -1, -1 );
                return;
            }
            if ( mLastClickedPanel.Item1 != -1 )
            {
                if ( mLastClickedPanel.Item2 == 0 ) //this is the Black
                {
                    mBlackFigureCollection[ mLastClickedPanel.Item1 ].borderColor = "White";
                }
                else
                {
                    mWhiteFigureCollection[ mLastClickedPanel.Item1 ].borderColor = "White";
                }
            }
            mLastClickedPanel = new Tuple<int, int>( aArguments.index, aArguments.y );
            if ( mLastClickedPanel.Item2 == 0 ) //this is the Black
            {
                mBlackFigureCollection[ mLastClickedPanel.Item1 ].borderColor = "Red";
            }
            else
            {
                mWhiteFigureCollection[ mLastClickedPanel.Item1 ].borderColor = "Red";
            }
            selectedType = getFigureType( aArguments.index, aArguments.y );
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        private FigureType getFigureType( Int32 aRow, Int32 aColor )
        {
            if (aColor == 0) //thats the Black
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
            panelSize = 6 * ( fieldSize + 15 );

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

        public ObservableCollection<BoardItem> mChessBoardCollection { get; set; }
        public ObservableCollection<BoardItem> mWhiteFigureCollection { get; set; }
        public ObservableCollection<BoardItem> mBlackFigureCollection { get; set; }

        public FigureType selectedType { get; set; }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        private Int32 mLastClicked;
        private Tuple<Int32, Int32> mLastClickedPanel;

        private String mWindowState;
        private Int32 mWindowWidth;
        private Int32 mWindowHeight;

        private Int32 mFieldSize;
        private Int32 mBoardSize;
        private Int32 mPanelSize;
    }
}
