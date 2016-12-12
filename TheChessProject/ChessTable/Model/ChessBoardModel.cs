using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTable.Model
{
    public class ChessBoardModel
    {
        public ChessBoardModel()
        {
            isGameReady = false;
        }

        public List<ModelItem> whiteFigures { get; set; }
        public List<ModelItem> blackFigures { get; set; }
        public Boolean isGameReady { get; set; }
    }
}
