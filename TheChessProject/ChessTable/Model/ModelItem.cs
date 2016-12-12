using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessTable.Common;

namespace ChessTable.Model
{
    public class ModelItem
    {
        public Int32 x { get; set; }
        public Int32 y { get; set; }
        public Int32 index { get; set; }
        public FigureType type { get; set; }
    }
}
