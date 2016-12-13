using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using ChessTable.Common;

namespace ChessTable.ViewModels.Converters
{
    public class PlayerToStartConverter : IValueConverter
    {
        public Object Convert( Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture )
        {
            if ( value == null || !( value is Player ) )
            {
                return Binding.DoNothing;
            }

            Player startingPlayer = ( Player )value;
            if ( startingPlayer == Player.PLAYER_ONE )
            {
                return "Player one";
            }
            else if ( startingPlayer == Player.PLAYER_TWO )
            {
                return "Player two";
            }
            else
            {
                return "";
            }
        }

        public Object ConvertBack( Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture )
        {
            if ( value == null || !( value is String ) )
            {
                return DependencyProperty.UnsetValue;
            }

            try
            {
                String startingPlayer = value as String;

                if ( startingPlayer == "Player one" )
                {
                    return Player.PLAYER_ONE;
                }
                else if ( startingPlayer == "Player two" )
                {
                    return Player.PLAYER_TWO;
                }

                return Player.NO_PLAYER;
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }
}
