﻿using ChessTable.Common;
using System;
using System.Windows;
using System.Windows.Data;

namespace ChessTable.ViewModels.Converters
{
	public class AlgorithmConverter : IValueConverter
	{
		public Object Convert( Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture )
		{
			if ( value == null || ! ( value is Algorithm ) )
			{
				return Binding.DoNothing;
			}

			Algorithm algorithm = ( Algorithm )value;

			switch ( algorithm )
			{
				case Algorithm.HUMAN:							return "Human";
				case Algorithm.RANDOM:							return "Random";
				case Algorithm.ALPHA_BETA:						return "Alpha-Beta";
				case Algorithm.ALPHA_BETA_RANDOM:				return "Alpha-Beta Random";
				case Algorithm.ALPHA_BETA_WITH_WEIGHT:			return "Alpha-Beta with weight";
				case Algorithm.ALPHA_BETA_RANDOM_WITH_WEIGHT:	return "Alpha-Beta Random with weight";
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
				String algorithm = value as String;

				if ( algorithm == "Human" )
				{
					return Algorithm.HUMAN;
				}
				else if ( algorithm == "Random" )
				{
					return Algorithm.RANDOM;
				}
				else if ( algorithm == "Alpha-Beta" )
				{
					return Algorithm.ALPHA_BETA;
				}
				else if ( algorithm == "Alpha-Beta Random" )
				{
					return Algorithm.ALPHA_BETA_RANDOM;
				}
				else if ( algorithm == "Alpha-Beta with weight" )
				{
					return Algorithm.ALPHA_BETA_WITH_WEIGHT;
				}
				else
				{
					return Algorithm.ALPHA_BETA_RANDOM_WITH_WEIGHT;
				}
			}
			catch
			{
				return DependencyProperty.UnsetValue;
			}
		}
	}
}
