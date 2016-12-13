﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTable.Common
{
    public class PutFigureOnTheTableEventArg
    {
        public Int32 x { get; set; }
        public Int32 y { get; set; }
        public Int32 index { get; set; }
        public Tuple<Colors, FigureType> type { get; set; }
        public Player player { get; set; }
    }
}
