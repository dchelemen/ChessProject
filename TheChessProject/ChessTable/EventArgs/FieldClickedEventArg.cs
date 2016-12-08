﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChessTable.EventArgs
{
    public class FieldClickedEventArg
    {
        public Int32 x { get; set; }
        public Int32 y { get; set; }
        public Int32 index { get; set; }
        public String borderColor { get; set; }
        public ViewModels.FigureType type { get; set; }
    }
}