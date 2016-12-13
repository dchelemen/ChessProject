using ChessTable.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ChessTable.ViewModels.Converters
{
    class ColorConverter : IValueConverter
    {
        public Object Convert( Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture )
        {
            if ( value == null || !( value is Colors ) )
            {
                return Binding.DoNothing;
            }

            Colors color = ( Colors )value;

            switch ( color )
            {
                case Colors.BLACK:  return "Black";
                case Colors.WHITE:  return "White";
                case Colors.RED:    return "Red";
                case Colors.GREEN:  return "Green";
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
                String color = value as String;

                if ( color == "Black" )
                {
                    return Colors.BLACK;
                }
                else if ( color == "White" )
                {
                    return Colors.WHITE;
                }
                else if ( color == "Red" )
                {
                    return Colors.RED;
                }
                else if ( color == "Green" )
                {
                    return Colors.GREEN;
                }

                return Colors.NO_COLOR;
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }
}
