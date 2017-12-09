using ChessTable.Common;
using System;
using System.Windows;
using System.Windows.Data;

namespace ChessTable.ViewModels.Converters
{
	class GameTypeConverter : IValueConverter
	{
		public Object Convert( Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture )
		{
			if ( value == null || ! ( value is GameType ) )
			{
				return Binding.DoNothing;
			}

			GameType gameType = ( GameType )value;

			switch ( gameType )
			{
				case GameType.STANDARD_GAME:	return "Standard Game";
				case GameType.CUSTOM_GAME:		return "Custom Game";
			}
			return Binding.DoNothing;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public Object ConvertBack( Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture )
		{
			if ( value == null || ! ( value is String ) )
			{
				return DependencyProperty.UnsetValue;
			}

			try
			{
				String color = value as String;

				if ( color == "Standard Game" )
				{
					return GameType.STANDARD_GAME;
				}
				else if ( color == "Custom Game" )
				{
					return GameType.CUSTOM_GAME;
				}

				return DependencyProperty.UnsetValue;
			}
			catch
			{
				return DependencyProperty.UnsetValue;
			}
		}
	}
}
