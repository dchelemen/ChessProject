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
			if ( value == null || ! ( value is FigureItem ) )
			{
				return Binding.DoNothing;
			}

			FigureItem figure = ( FigureItem )value;
			if ( figure.color == Colors.BLACK )
			{
				switch ( figure.figureType )
				{
					case FigureType.MOVED_KING:
					case FigureType.KING:	return "/Images/Black_King.png";
					case FigureType.QUEEN:	return "/Images/Black_Queen.png";
					case FigureType.MOVED_ROOK:
					case FigureType.ROOK:	return "/Images/Black_Rook.png";
					case FigureType.BISHOP:	return "/Images/Black_Bishop.png";
					case FigureType.KNIGHT:	return "/Images/Black_Knight.png";
					case FigureType.PAWN:	return "/Images/Black_Pawn.png";
				}
			}
			else
			{
				switch ( figure.figureType )
				{
					case FigureType.MOVED_KING:
					case FigureType.KING:	return "/Images/White_King.png";
					case FigureType.QUEEN:	return "/Images/White_Queen.png";
					case FigureType.MOVED_ROOK:
					case FigureType.ROOK:	return "/Images/White_Rook.png";
					case FigureType.BISHOP:	return "/Images/White_Bishop.png";
					case FigureType.KNIGHT:	return "/Images/White_Knight.png";
					case FigureType.PAWN:	return "/Images/White_Pawn.png";
				}
			}

			if ( figure.color == Colors.NO_COLOR || figure.figureType == FigureType.NO_FIGURE )
			{
				return DependencyProperty.UnsetValue;
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
				String figureType = value as String;

				if ( figureType == "/Images/Black_King.png" )
				{
					return new FigureItem( Colors.BLACK, FigureType.KING );
				}
				else if ( figureType == "/Images/Black_Queen.png" )
				{
					return new FigureItem( Colors.BLACK, FigureType.QUEEN );
				}
				else if ( figureType == "/Images/Black_Rook.png" )
				{
					return new FigureItem( Colors.BLACK, FigureType.ROOK );
				}
				else if ( figureType == "/Images/Black_Bishop.png" )
				{
					return new FigureItem( Colors.BLACK, FigureType.BISHOP );
				}
				else if ( figureType == "/Images/Black_Knight.png" )
				{
					return new FigureItem( Colors.BLACK, FigureType.KNIGHT );
				}
				else if ( figureType == "/Images/Black_Pawn.png" )
				{
					return new FigureItem( Colors.BLACK, FigureType.PAWN );
				}
				else if ( figureType == "/Images/White_King.png" )
				{
					return new FigureItem( Colors.WHITE, FigureType.KING );
				}
				else if ( figureType == "/Images/White_Queen.png" )
				{
					return new FigureItem( Colors.WHITE, FigureType.QUEEN );
				}
				else if ( figureType == "/Images/White_Rook.png" )
				{
					return new FigureItem( Colors.WHITE, FigureType.ROOK );
				}
				else if ( figureType == "/Images/White_Bishop.png" )
				{
					return new FigureItem( Colors.WHITE, FigureType.BISHOP );
				}
				else if ( figureType == "/Images/White_Knight.png" )
				{
					return new FigureItem( Colors.WHITE, FigureType.KNIGHT );
				}
				else if ( figureType == "/Images/White_Pawn.png" )
				{
					return new FigureItem( Colors.WHITE, FigureType.PAWN );
				}

				return new FigureItem( Colors.NO_COLOR, FigureType.NO_FIGURE );
			}
			catch
			{
				return DependencyProperty.UnsetValue;
			}
		}
	}
}