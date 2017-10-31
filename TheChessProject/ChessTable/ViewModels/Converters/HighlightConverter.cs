using ChessTable.Common;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ChessTable.ViewModels.Converters
{
	public class HighlightConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ( value == null || ! ( value is Colors ) )
			{
				return Binding.DoNothing;
			}

			Colors color = ( Colors )value;
			switch ( color )
			{
				case Colors.BLUE:		return "/Images/Blue_Highlight.png";
				case Colors.RED:		return "/Images/Red_Highlight.png";
				case Colors.NO_COLOR:	return DependencyProperty.UnsetValue;
			}

			return Binding.DoNothing;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || ! ( value is String ) )
			{
				return DependencyProperty.UnsetValue;
			}

			try
			{
				String figureType = value as String;

				if ( figureType == "/Images/Blue_Highlight.png" )
				{
					return Colors.BLUE;
				}
				else if ( figureType == "/Images/Red_Highlight.png" )
				{
					return Colors.RED;
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
