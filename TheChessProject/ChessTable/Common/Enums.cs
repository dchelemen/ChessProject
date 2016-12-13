using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTable.Common
{
    public enum FigureType
    {
        KING,
        QUEEN,
        ROOK,
        BISHOP,
        KNIGHT,
        PAWN,
        NO_FIGURE
    }

    public enum Colors
    {
        WHITE,
        BLACK,
        RED,
        GREEN,
        NO_COLOR
    }

    public enum Player
    {
        PLAYER_ONE,
        PLAYER_TWO,
        NO_PLAYER
    }

    public enum Algorithm
    {
        HUMAN,
        RANDOM,
        ALPHA_BETA
    }
}
