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

		public abstract void setTree( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures );

		//----------------------------------------------------------------------------------------------------------------------------------------

		public abstract void refreshTree( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures, Move aLastMove );

		//----------------------------------------------------------------------------------------------------------------------------------------

		public abstract Move move( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures );

		//----------------------------------------------------------------------------------------------------------------------------------------
		protected List< Int32 > possibleMoves( List< List< ModelItem > > aChessBoard, List< ModelItem > aBlackFigures, List< ModelItem > aWhiteFigures, ModelItem currentFigure)
		{
			List< Int32 > possibleMoves = new List< Int32 >();
			switch ( currentFigure.figureItem.figureType )
			{
			case FigureType.KING:
				{
					KingRule kingRule		= new KingRule( aChessBoard, aWhiteFigures, aBlackFigures, player1Color, currentFigure, true );
					possibleMoves			= kingRule.possibleMoves();
				} break;
			case FigureType.QUEEN:
				{
					QueenRule queenRule		= new QueenRule( aChessBoard, aWhiteFigures, aBlackFigures, player1Color, currentFigure, true );
					possibleMoves			= queenRule.possibleMoves();
				} break;
			case FigureType.ROOK:
				{
					RookRule rookRule		= new RookRule( aChessBoard, aWhiteFigures, aBlackFigures, player1Color, currentFigure, true );
					possibleMoves			= rookRule.possibleMoves();
				} break;
			case FigureType.BISHOP:
				{
					BishopRule bishopRule	= new BishopRule( aChessBoard, aWhiteFigures, aBlackFigures, player1Color, currentFigure, true );
					possibleMoves			= bishopRule.possibleMoves();
				} break;
			case FigureType.KNIGHT:
				{
					KnightRule knightRule	= new KnightRule( aChessBoard, aWhiteFigures, aBlackFigures, player1Color, currentFigure, true );
					possibleMoves			= knightRule.possibleMoves();
				} break;
			case FigureType.PAWN:
				{
					PawnRule pawnRule		= new PawnRule( aChessBoard, aWhiteFigures, aBlackFigures, player1Color, currentFigure, true );
					possibleMoves			= pawnRule.possibleMoves();
				} break;
			case FigureType.NO_FIGURE:		break;
			}

			return possibleMoves;
		}
		
		//----------------------------------------------------------------------------------------------------------------------------------------

		protected FigureItem getFigureItemFromPosition( Int16 aFigureItem )
		{
			switch ( aFigureItem )
			{
			case 1:		return new FigureItem( Colors.WHITE,	FigureType.PAWN			);
			case 2:		return new FigureItem( Colors.WHITE,	FigureType.PAWN			);
			case 3:		return new FigureItem( Colors.WHITE,	FigureType.KNIGHT		);
			case 4:		return new FigureItem( Colors.WHITE,	FigureType.BISHOP		);
			case 5:		return new FigureItem( Colors.WHITE,	FigureType.ROOK			);
			case 9:		return new FigureItem( Colors.WHITE,	FigureType.QUEEN		);
			case 10:	return new FigureItem( Colors.WHITE,	FigureType.KING			);

			case -1:	return new FigureItem( Colors.BLACK,	FigureType.PAWN			);
			case -2:	return new FigureItem( Colors.BLACK,	FigureType.PAWN			);
			case -3:	return new FigureItem( Colors.BLACK,	FigureType.KNIGHT		);
			case -4:	return new FigureItem( Colors.BLACK,	FigureType.BISHOP		);
			case -5:	return new FigureItem( Colors.BLACK,	FigureType.ROOK			);
			case -9:	return new FigureItem( Colors.BLACK,	FigureType.QUEEN		);
			case -10:	return new FigureItem( Colors.BLACK,	FigureType.KING			);

			default:	return new FigureItem( Colors.NO_COLOR,	FigureType.NO_FIGURE	);
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		protected List< TreeNode > mTree { get; set; }
		public Boolean isActive { get; set; }
		protected Colors player1Color { get; set; }
		protected Colors myColor { get; set; }
	}
}
