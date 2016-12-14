using System;
using ChessTable.Common;

namespace ChessTable.Model
{
    public class ModelItem
    {
        public Int32 x { get; set; }
        public Int32 y { get; set; }
        public Int32 index { get; set; }
        public Tuple<Colors, FigureType> type { get; set; }
        public Player player { get; set; }
    }
}
