using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChessTable.ViewModels
{
    public class BoardItem : ViewModelBase
    {
        public BoardItem()
        {
            onFieldClickedCommand = new DelegateCommand( x => onFieldClicked() );
        }

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
        public void onFieldClicked()
        {
            FieldFigure = typesToString( mSelectedType );
        }


        public String FieldColor
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
                    OnPropertyChanged( "FieldColor" );
                }
            }
        }

        public String FieldFigure
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
                    OnPropertyChanged( "FieldFigure" );
                }
            }
        }

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

        public DelegateCommand onFieldClickedCommand { get; private set; }
        public Int32 X { get; set; }

        public Int32 Y { get; set; }

        public static FigureType mSelectedType = FigureType.NO_FIGURE;

        private String mFieldColor;
        private String mFieldFigure;
        private Int32 mFieldSize;
    }
}
