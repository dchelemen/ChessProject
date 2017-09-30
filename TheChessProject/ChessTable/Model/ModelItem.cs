using System;
using ChessTable.Common;

namespace ChessTable.Model
{
	public class ModelItem : IEquatable< ModelItem >
	{
		public ModelItem()
		{
			x			= -1;
			y			= -1;
			figureItem	= new FigureItem( Colors.NO_COLOR, FigureType.NO_FIGURE );
			index		= -1;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public ModelItem( Int32 aX, Int32 aY, Colors aColor, FigureType aFigureType )
		{
			x			= aX;
			y			= aY;
			figureItem	= new FigureItem( aColor, aFigureType );
			index		= ( aX * 8 + aY );
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override bool Equals( System.Object obj )
		{
			// If parameter is null return false.
			if (obj == null)
			{
				return false;
			}

			// If parameter cannot be cast to Point return false.
			ModelItem aOther = obj as ModelItem;
			if ((System.Object)aOther == null)
			{
				return false;
			}

			// Return true if the fields match:
			return ( this == aOther );
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public bool Equals( ModelItem aOther )
		{
			// If both are null, or both are same instance, return true.
			if ( ( object )aOther == null )
			{
				return false;
			}

			// Return true if the fields match:
			return ( this == aOther );
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public static Boolean operator==( ModelItem aLeftItem, ModelItem aRightItem )
		{
			bool xEqual				= aLeftItem.x == aRightItem.x;
			bool yEqual				= aLeftItem.y == aRightItem.y;
			bool indexEqual			= aLeftItem.index == aRightItem.index;
			bool figureItemEqual	= aLeftItem.figureItem.color == aRightItem.figureItem.color && aLeftItem.figureItem.figureType == aRightItem.figureItem.figureType;

			return ( xEqual && yEqual && indexEqual && figureItemEqual );
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public static Boolean operator!=( ModelItem aLeftItem, ModelItem aRightItem )
		{
			return !( aLeftItem == aRightItem );
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override int GetHashCode()
		{
			return index;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public Int32									x { get; set; }
		public Int32									y { get; set; }
		public Int32									index { get; set; }
		public FigureItem								figureItem { get; set; }

	}
}
