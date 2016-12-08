using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessTable.View;
using System.Windows;
using System.Collections.ObjectModel;

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
            WindowWidth     = 640;
            WindowHeight    = 480;
            FieldSize       = 48;
            BoardSize       = 384;
            onBlackPawnClickedCommand = new DelegateCommand( x => onBlackPawnClicked() );
            ChessBoard = new ObservableCollection< BoardItem >();
            setupBoardColor();
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        public void onBlackPawnClicked()
        {
            MessageBox.Show( "Te Paraszt!" );
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// creates the board colors (black and white)
        /// </summary>
        private void setupBoardColor()
        {
            ChessBoard.Clear();
            String color;
            for ( Int16 row = 0; row < 8; row++ )
            {
                for ( Int16 column = 0; column < 8; column++ )
                {
                    color = ( row + column ) % 2 == 0 ? "White" : "Black";
                    ChessBoard.Add( new BoardItem()
                    {
                        X = row,
                        Y = column,
                        FieldColor = color,
                        FieldFigure = "",
                        FieldSize = 48
                    } );
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        private void calculateFieldSize()
        {
            Int32 minSize = WindowHeight < WindowWidth ? WindowHeight : WindowWidth;
            FieldSize = minSize / 10;
            BoardSize = 8 * FieldSize;

            if ( ChessBoard != null )
            {
                foreach ( BoardItem item in ChessBoard )
                {
                    item.FieldSize = FieldSize;
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        public String BlackPawn
        {
            get
            {
                return mBlackPawn;
            }
            set
            {
                if ( mBlackPawn != value )
                {
                    mBlackPawn = value;
                    OnPropertyChanged( "BlackPawn" );
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        public Int32 WindowWidth
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
                    OnPropertyChanged( "WindowWidth" );
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        public Int32 WindowHeight
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
                    OnPropertyChanged( "WindowHeight" );
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        public Int32 FieldSize
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
                    OnPropertyChanged( "FieldSize" );
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        public Int32 BoardSize
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
                    OnPropertyChanged( "BoardSize" );
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        public DelegateCommand onBlackPawnClickedCommand { get; private set; }

        public ObservableCollection<BoardItem> ChessBoard { get; set; }

        public FigureType selectedType { get; set; }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        private String mBlackPawn;
        private Int32 mWindowWidth;
        private Int32 mWindowHeight;
        private Int32 mFieldSize;
        private Int32 mBoardSize;
    }
}
