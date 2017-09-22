using System;


namespace ChessTable.Common
{
	public class FieldClickedEventArg
	{
		public Int32 x { get; set; }
		public Int32 y { get; set; }
		public Int32 index { get; set; }
		public Tuple< Colors, FigureType > type { get; set; }
	}
}
