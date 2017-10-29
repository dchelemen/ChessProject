using System;

namespace ChessTable.Common
{
	public class PutFigureOnTheTableEventArg
	{
		public PutFigureOnTheTableEventArg(){}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public PutFigureOnTheTableEventArg( Int32 aX, Int32 aY, Int32 aIndex, Colors aFigureColor, FigureType aFigureType )
		{
			x			= aX;
			y			= aY;
			index		= aIndex;
			figureItem	= new FigureItem( aFigureColor, aFigureType );
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public Int32		x { get; set; }
		public Int32		y { get; set; }
		public Int32		index { get; set; }
		public FigureItem	figureItem { get; set; }
	}
}
