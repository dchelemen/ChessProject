using System;
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
        public CustomBoardViewModel( Colors aPlayer1Color )
        {
            mPlayer1Color = aPlayer1Color;
            windowState     = "Normal";
            windowWidth     = 640;
            windowHeight    = 480;
            fieldSize       = 48;
            boardSize       = 384;

            selectedPanelItem = new Tuple<Colors, FigureType>( Colors.NO_COLOR, FigureType.NO_FIGURE );
            mLastClickedField = -1;

            mChessBoardCollection   = new ObservableCollection< BoardItem >();
            mBlackFigureCollection  = new ObservableCollection< BoardItem >();
            mWhiteFigureCollection  = new ObservableCollection< BoardItem >();
            mChessBoardModel        = new ChessBoardModel( mPlayer1Color );

            setupCustomBoard();
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
                    mChessBoardCollection.Add( new BoardItem()
                    {
                        X = row,
                        Y = column,
                        Index = index,
                        fieldColor = color,
                        fieldSize = 48,
                        figureType = new Tuple<Colors, FigureType>( Colors.NO_COLOR, FigureType.NO_FIGURE ),
                        borderColor = color
                    } );
                    mChessBoardCollection[ index ].fieldClicked += new EventHandler< FieldClickedEventArg >( onFieldClicked );
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
                    fieldColor = Colors.BLACK,
                    fieldSize = 48,
                    borderColor = Colors.WHITE,
                    Color = Colors.BLACK
                } );

                mWhiteFigureCollection.Add( new BoardItem
                {
                    X = row,
                    Y = 1, //its for White
                    Index = row,
                    fieldColor = Colors.BLACK,
                    fieldSize = 48,
                    borderColor = Colors.WHITE,
                    Color = Colors.WHITE
                } );

                mBlackFigureCollection[ row ].fieldClicked += new EventHandler< FieldClickedEventArg >( onPanelClicked );
                mWhiteFigureCollection[ row ].fieldClicked += new EventHandler< FieldClickedEventArg >( onPanelClicked );
            }

            mBlackFigureCollection[ 0 ].figureType = new Tuple<Colors, FigureType>( Colors.BLACK ,FigureType.KING   );
            mBlackFigureCollection[ 1 ].figureType = new Tuple<Colors, FigureType>( Colors.BLACK, FigureType.QUEEN  );
            mBlackFigureCollection[ 2 ].figureType = new Tuple<Colors, FigureType>( Colors.BLACK, FigureType.ROOK   );
            mBlackFigureCollection[ 3 ].figureType = new Tuple<Colors, FigureType>( Colors.BLACK, FigureType.BISHOP );
            mBlackFigureCollection[ 4 ].figureType = new Tuple<Colors, FigureType>( Colors.BLACK, FigureType.KNIGHT );
            mBlackFigureCollection[ 5 ].figureType = new Tuple<Colors, FigureType>( Colors.BLACK, FigureType.PAWN   );

            mWhiteFigureCollection[ 0 ].figureType = new Tuple<Colors, FigureType>( Colors.WHITE, FigureType.KING   );
            mWhiteFigureCollection[ 1 ].figureType = new Tuple<Colors, FigureType>( Colors.WHITE, FigureType.QUEEN  );
            mWhiteFigureCollection[ 2 ].figureType = new Tuple<Colors, FigureType>( Colors.WHITE, FigureType.ROOK   );
            mWhiteFigureCollection[ 3 ].figureType = new Tuple<Colors, FigureType>( Colors.WHITE, FigureType.BISHOP );
            mWhiteFigureCollection[ 4 ].figureType = new Tuple<Colors, FigureType>( Colors.WHITE, FigureType.KNIGHT );
            mWhiteFigureCollection[ 5 ].figureType = new Tuple<Colors, FigureType>( Colors.WHITE, FigureType.PAWN   );
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        private void onFieldClicked(Object aSender, FieldClickedEventArg aArguments )
        {
            if ( selectedPanelItem.Item2 == FigureType.NO_FIGURE )
            {
                return;
            }
            if ( mLastClickedField != -1 )
            {
                mChessBoardCollection[ mLastClickedField ].borderColor = ( mChessBoardCollection[ mLastClickedField ].X + mChessBoardCollection[ mLastClickedField ].Y ) % 2 == 0 ? Colors.WHITE : Colors.BLACK;
                mLastClickedField = -1;
            }
            mChessBoardCollection[ aArguments.index ].figureType = selectedPanelItem;
            mChessBoardCollection[ aArguments.index ].borderColor = Colors.RED;

            foreach ( ModelItem item in mChessBoardModel.blackFigures )
            {
                if ( item.index == aArguments.index )
                {
                    mChessBoardModel.blackFigures.Remove( item );
                    break;
                }
            }
            foreach( ModelItem item in mChessBoardModel.whiteFigures )
            {
                if ( item.index == aArguments.index )
                {
                    mChessBoardModel.whiteFigures.Remove( item );
                    break;
                }
            }

            if ( selectedPanelItem.Item1 == Colors.BLACK )
            {
                mChessBoardModel.blackFigures.Add( new ModelItem
                {
                    player = mPlayer1Color == Colors.BLACK ? Player.PLAYER_ONE : Player.PLAYER_TWO,
                    type = selectedPanelItem,
                    x = aArguments.x,
                    y = aArguments.y,
                    index = aArguments.index
                } );
            }
            else
            {
                mChessBoardModel.whiteFigures.Add( new ModelItem
                {
                    player = mPlayer1Color == Colors.WHITE ? Player.PLAYER_ONE : Player.PLAYER_TWO,
                    type = selectedPanelItem,
                    x = aArguments.x,
                    y = aArguments.y,
                    index = aArguments.index
                } );
            }

            mLastClickedField = aArguments.index;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        private void onPanelClicked( Object aSender, FieldClickedEventArg aArguments )
        {
            if ( selectedPanelItem == aArguments.type )
            {
                mBlackFigureCollection.Concat( mWhiteFigureCollection ).Where( X => X.figureType == selectedPanelItem ).FirstOrDefault().borderColor = Colors.WHITE;
                selectedPanelItem = new Tuple<Colors, FigureType>( Colors.NO_COLOR, FigureType.NO_FIGURE );
                return;
            }
            if ( selectedPanelItem.Item2 != FigureType.NO_FIGURE )
            {
                mBlackFigureCollection.Concat( mWhiteFigureCollection ).Where( X => X.figureType == selectedPanelItem ).FirstOrDefault().borderColor = Colors.WHITE;
            }

            selectedPanelItem = aArguments.type;

            mBlackFigureCollection.Concat( mWhiteFigureCollection ).Where( X => X.figureType == selectedPanelItem ).FirstOrDefault().borderColor = Colors.RED;
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

        public ChessBoardModel chessBoardModel
        {
            get
            {
                return mChessBoardModel;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        public ObservableCollection<BoardItem> mChessBoardCollection { get; set; }
        public ObservableCollection<BoardItem> mWhiteFigureCollection { get; set; }
        public ObservableCollection<BoardItem> mBlackFigureCollection { get; set; }

        private Tuple<Colors,FigureType> selectedPanelItem { get; set; }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        private Int32 mLastClickedField;

        private ChessBoardModel mChessBoardModel;

        private Colors mPlayer1Color;

        private String mWindowState;
        private Int32 mWindowWidth;
        private Int32 mWindowHeight;

        private Int32 mFieldSize;
        private Int32 mBoardSize;
        private Int32 mPanelSize;
    }
}
