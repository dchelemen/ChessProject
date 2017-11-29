using ChessTable.Common;
using ChessTable.Model.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTable.Model.Algorithms
{
	public abstract class BaseAlgorithm
	{
		public BaseAlgorithm( Colors aPlayer1Color, Colors aMyColor )
		{
			player1Color	= aPlayer1Color;
			myColor			= aMyColor;
			mTree = null;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public abstract void setTree( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures, CastlingRule aCastlingRule, ChessRule aChess );

		//----------------------------------------------------------------------------------------------------------------------------------------

		public abstract void refreshTree( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures, CastlingRule aCastlingRule, Move aLastMove, ChessRule aChess );

		//----------------------------------------------------------------------------------------------------------------------------------------

		public abstract Move nextMove( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures, CastlingRule aCastlingRule );

		//----------------------------------------------------------------------------------------------------------------------------------------
		protected List< Int32 > possibleMoves( List< List< ModelItem > > aChessBoard, List< ModelItem > aBlackFigures, List< ModelItem > aWhiteFigures, CastlingRule aCastlingRule, ModelItem currentFigure, ChessRule aChess )
		{
			List< Int32 > possibleMoves = new List< Int32 >();
			switch ( currentFigure.figureItem.figureType )
			{
			case FigureType.KING:
				{
					KingRule kingRule		= new KingRule( aChessBoard, player1Color, currentFigure, aBlackFigures, aWhiteFigures, aCastlingRule );
					possibleMoves			= kingRule.possibleMoves( aChess );
				} break;
			case FigureType.QUEEN:
				{
					QueenRule queenRule		= new QueenRule( aChessBoard, player1Color, currentFigure );
					possibleMoves			= queenRule.possibleMoves( aChess );
				} break;
			case FigureType.ROOK:
				{
					RookRule rookRule		= new RookRule( aChessBoard, player1Color, currentFigure );
					possibleMoves			= rookRule.possibleMoves( aChess );
				} break;
			case FigureType.BISHOP:
				{
					BishopRule bishopRule	= new BishopRule( aChessBoard, player1Color, currentFigure );
					possibleMoves			= bishopRule.possibleMoves( aChess );
				} break;
			case FigureType.KNIGHT:
				{
					KnightRule knightRule	= new KnightRule( aChessBoard, player1Color, currentFigure );
					possibleMoves			= knightRule.possibleMoves( aChess );
				} break;
			case FigureType.PAWN:
				{
					PawnRule pawnRule		= new PawnRule( aChessBoard, player1Color, currentFigure );
					possibleMoves			= pawnRule.possibleMoves( aChess );
				} break;
			case FigureType.NO_FIGURE:		break;
			}

			return possibleMoves;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		protected List< TreeNode > mTree { get; set; }
		public Boolean isActive { get; set; }
		protected Colors player1Color { get; set; }
		protected Colors myColor { get; set; }
	}
}
