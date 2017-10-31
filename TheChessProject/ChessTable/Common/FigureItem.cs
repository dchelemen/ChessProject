namespace ChessTable.Common
{
	public class FigureItem
	{
		public FigureItem(){}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public FigureItem( Colors aColor, FigureType aType )
		{
			color		= aColor;
			figureType	= aType;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public Colors			color { get; set; }
		public FigureType		figureType { get; set; }
	}
}
