using ChessTable.Common;
using System;
using System.Collections.Generic;

namespace ChessTable.Model
{
    public class ChessBoardModel
    {
        public ChessBoardModel( Colors aPlayer1Color )
        {
            whiteFigures = new List<ModelItem>();
            blackFigures = new List<ModelItem>();
            mPlayer1Color = aPlayer1Color;
            isGameReady = false;
        }

        public void startModel()
        {
            foreach ( ModelItem item in whiteFigures )
            {
                fieldClicked( this, new PutFigureOnTheTableEventArg
                {
                    player      = item.player,
                    type        = item.type,
                    x           = item.x,
                    y           = item.y,
                    index       = item.index
                } );
            }
            foreach ( ModelItem item in blackFigures )
            {
                fieldClicked( this, new PutFigureOnTheTableEventArg
                {
                    player = item.player,
                    type = item.type,
                    x = item.x,
                    y = item.y,
                    index = item.index
                } );
            }
        }

        public event EventHandler<PutFigureOnTheTableEventArg> fieldClicked;
        public List<ModelItem> whiteFigures { get; set; }
        public List<ModelItem> blackFigures { get; set; }
        public Boolean isGameReady { get; set; }

        private Colors mPlayer1Color;
    }
}
