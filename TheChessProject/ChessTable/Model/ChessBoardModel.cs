using ChessTable.Common;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ChessTable.Model
{
    public class ChessBoardModel
    {
        public ChessBoardModel( Colors aPlayer1Color, Colors aStartingColor, Algorithm aPlayer1Algorithm, Algorithm aPlayer2Algorithm )
        {
            mPlayer1Color       = aPlayer1Color;
            mPlayer1Algorithm   = aPlayer1Algorithm;
            mPlayer2Algorithm   = aPlayer2Algorithm;
            mCurrentColor       = aStartingColor;

            whiteFigures = new List< ModelItem >();
            blackFigures = new List< ModelItem >();

            isGameReady = false;
        }

        public void startModel()
        {
            chessBoard = new List<List<ModelItem>>();

            for ( Int32 row = 0; row < 8; row++ )
            {
                List< ModelItem > chessBoardRow = new List< ModelItem >();
                for ( Int32 column = 0; column < 8; column++ )
                {
                    chessBoardRow.Add( new ModelItem
                    {
                        index   = row * 8 + column,
                        x       = row,
                        y       = column,
                        type    = new Tuple<Colors, FigureType>( Colors.NO_COLOR, FigureType.NO_FIGURE )
                    } );
                }
                chessBoard.Add( chessBoardRow );
            }

            foreach ( ModelItem item in whiteFigures )
            {
                chessBoard[ item.x ][ item.y ] = item;
                fieldClicked( this, new PutFigureOnTheTableEventArg
                {
                    type    = item.type,
                    x       = item.x,
                    y       = item.y,
                    index   = item.index
                } );
            }

            foreach ( ModelItem item in blackFigures )
            {
                chessBoard[ item.x ][ item.y ] = item;
                fieldClicked( this, new PutFigureOnTheTableEventArg
                {
                    type    = item.type,
                    x       = item.x,
                    y       = item.y,
                    index   = item.index
                } );
            }
        }

        public event EventHandler<PutFigureOnTheTableEventArg> fieldClicked;
        public List<ModelItem> whiteFigures { get; set; }
        public List<ModelItem> blackFigures { get; set; }
        private List< List< ModelItem > > chessBoard { get; set; }
        public Boolean isGameReady { get; set; }

        private Colors mPlayer1Color;
        private Algorithm mPlayer1Algorithm;
        private Algorithm mPlayer2Algorithm;
        private Colors mCurrentColor;
    }
}
