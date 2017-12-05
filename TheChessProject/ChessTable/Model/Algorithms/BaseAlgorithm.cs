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
			mTreeRoot		= new TreeNode( null );
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public abstract void setTree( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures );

		//----------------------------------------------------------------------------------------------------------------------------------------

		public abstract void refreshTree( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures, Move aLastMove );

		//----------------------------------------------------------------------------------------------------------------------------------------

		public abstract void move( List< List< ModelItem > > aChessBoard, List< ModelItem > aWhiteFigures, List< ModelItem > aBlackFigures );

		//----------------------------------------------------------------------------------------------------------------------------------------

		protected List< Int32 > possibleMoves( List< List< ModelItem > > aChessBoard, List< ModelItem > aBlackFigures, List< ModelItem > aWhiteFigures, ModelItem currentFigure )
		{
			List< Int32 > possibleMoves = new List< Int32 >();
			switch ( currentFigure.figureItem.figureType )
			{
			case FigureType.MOVED_KING:
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
			case FigureType.MOVED_ROOK:
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
			case 1:		return new FigureItem( Colors.WHITE,	FigureType.PAWN				);
			case 2:		return new FigureItem( Colors.WHITE,	FigureType.EN_PASSANT_PAWN	);
			case 3:		return new FigureItem( Colors.WHITE,	FigureType.KNIGHT			);
			case 4:		return new FigureItem( Colors.WHITE,	FigureType.BISHOP			);
			case 5:		return new FigureItem( Colors.WHITE,	FigureType.ROOK				);
			case 6:		return new FigureItem( Colors.WHITE,	FigureType.MOVED_ROOK		);
			case 9:		return new FigureItem( Colors.WHITE,	FigureType.QUEEN			);
			case 10:	return new FigureItem( Colors.WHITE,	FigureType.KING				);
			case 11:	return new FigureItem( Colors.WHITE,	FigureType.MOVED_KING		);

			case -1:	return new FigureItem( Colors.BLACK,	FigureType.PAWN				);
			case -2:	return new FigureItem( Colors.BLACK,	FigureType.EN_PASSANT_PAWN	);
			case -3:	return new FigureItem( Colors.BLACK,	FigureType.KNIGHT			);
			case -4:	return new FigureItem( Colors.BLACK,	FigureType.BISHOP			);
			case -5:	return new FigureItem( Colors.BLACK,	FigureType.ROOK				);
			case -6:	return new FigureItem( Colors.BLACK,	FigureType.MOVED_ROOK		);
			case -9:	return new FigureItem( Colors.BLACK,	FigureType.QUEEN			);
			case -10:	return new FigureItem( Colors.BLACK,	FigureType.KING				);
			case -11:	return new FigureItem( Colors.BLACK,	FigureType.MOVED_KING		);

			default:	return new FigureItem( Colors.NO_COLOR,	FigureType.NO_FIGURE		);
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		protected Int16 getFigureID( ModelItem aModelItem )
		{
			Int16 whiteOrBlack = ( Int16 )( aModelItem.figureItem.color == Colors.WHITE ? 1 : -1 );

			switch ( aModelItem.figureItem.figureType )
			{
			case FigureType.PAWN:				return ( Int16 )( 1 * whiteOrBlack );
			case FigureType.EN_PASSANT_PAWN:	return ( Int16 )( 2 * whiteOrBlack );
			case FigureType.KNIGHT:				return ( Int16 )( 3 * whiteOrBlack );
			case FigureType.BISHOP:				return ( Int16 )( 4 * whiteOrBlack );
			case FigureType.ROOK:				return ( Int16 )( 5 * whiteOrBlack );
			case FigureType.MOVED_ROOK:			return ( Int16 )( 6 * whiteOrBlack );
			case FigureType.QUEEN:				return ( Int16 )( 9 * whiteOrBlack );
			case FigureType.KING:				return ( Int16 )( 10 * whiteOrBlack );
			case FigureType.MOVED_KING:			return ( Int16 )( 11 * whiteOrBlack );
			default: return 0;
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		protected TreeNode mTreeRoot { get; set; }
		public Boolean isActive { get; set; }
		protected Colors player1Color { get; set; }
		protected Colors myColor { get; set; }
	}
}
