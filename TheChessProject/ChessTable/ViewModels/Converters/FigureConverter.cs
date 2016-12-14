using ChessTable.Common;
using System;
using System.Windows;
using System.Windows.Data;

namespace ChessTable.ViewModels.Converters
{
    class FigureConverter : IValueConverter
    {
        public Object Convert( Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture )
        {
            if ( value == null || !( value is Tuple<Colors, FigureType> ) )
            {
                return Binding.DoNothing;
            }

            Tuple<Colors, FigureType> figureType = ( Tuple<Colors, FigureType> )value;
            if ( figureType.Item1 == Colors.BLACK )
            {
                switch ( figureType.Item2 )
                {
                    case FigureType.KING:   return "/Images/Black_King.png";
                    case FigureType.QUEEN:  return "/Images/Black_Queen.png";
                    case FigureType.ROOK:   return "/Images/Black_Rook.png";
                    case FigureType.BISHOP: return "/Images/Black_Bishop.png";
                    case FigureType.KNIGHT: return "/Images/Black_Knight.png";
                    case FigureType.PAWN:   return "/Images/Black_Pawn.png";
                }
            }
            else
            {
                switch ( figureType.Item2 )
                {
                    case FigureType.KING:   return "/Images/White_King.png";
                    case FigureType.QUEEN:  return "/Images/White_Queen.png";
                    case FigureType.ROOK:   return "/Images/White_Rook.png";
                    case FigureType.BISHOP: return "/Images/White_Bishop.png";
                    case FigureType.KNIGHT: return "/Images/White_Knight.png";
                    case FigureType.PAWN:   return "/Images/White_Pawn.png";
                }
            }

            if ( figureType.Item1 == Colors.NO_COLOR || figureType.Item2 == FigureType.NO_FIGURE )
            {
                return "";
            }

            return Binding.DoNothing;
        }

        public Object ConvertBack( Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture )
        {
            if ( value == null || !( value is String ) )
            {
                return DependencyProperty.UnsetValue;
            }

            try
            {
                String figureType = value as String;

                if ( figureType == "/Images/Black_King.png" )
                {
                    return new Tuple<Colors, FigureType>( Colors.BLACK, FigureType.KING );
                }
                else if ( figureType == "/Images/Black_Queen.png" )
                {
                    return new Tuple<Colors, FigureType>( Colors.BLACK, FigureType.QUEEN );
                }
                else if ( figureType == "/Images/Black_Rook.png" )
                {
                    return new Tuple<Colors, FigureType>( Colors.BLACK, FigureType.ROOK );
                }
                else if ( figureType == "/Images/Black_Bishop.png" )
                {
                    return new Tuple<Colors, FigureType>( Colors.BLACK, FigureType.BISHOP );
                }
                else if ( figureType == "/Images/Black_Knight.png" )
                {
                    return new Tuple<Colors, FigureType>( Colors.BLACK, FigureType.KNIGHT );
                }
                else if ( figureType == "/Images/Black_Pawn.png" )
                {
                    return new Tuple<Colors, FigureType>( Colors.BLACK, FigureType.PAWN );
                }
                else if ( figureType == "/Images/White_King.png" )
                {
                    return new Tuple<Colors, FigureType>( Colors.WHITE, FigureType.KING );
                }
                else if ( figureType == "/Images/White_Queen.png" )
                {
                    return new Tuple<Colors, FigureType>( Colors.WHITE, FigureType.QUEEN );
                }
                else if ( figureType == "/Images/White_Rook.png" )
                {
                    return new Tuple<Colors, FigureType>( Colors.WHITE, FigureType.ROOK );
                }
                else if ( figureType == "/Images/White_Bishop.png" )
                {
                    return new Tuple<Colors, FigureType>( Colors.WHITE, FigureType.BISHOP );
                }
                else if ( figureType == "/Images/White_Knight.png" )
                {
                    return new Tuple<Colors, FigureType>( Colors.WHITE, FigureType.KNIGHT );
                }
                else if ( figureType == "/Images/White_Pawn.png" )
                {
                    return new Tuple<Colors, FigureType>( Colors.WHITE, FigureType.PAWN );
                }

                return new Tuple< Colors, FigureType > ( Colors.NO_COLOR, FigureType.NO_FIGURE );
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }
}