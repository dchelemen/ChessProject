using ChessTable.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTable.Model.Rules
{
	public class ChessRule
	{
		public ChessRule()
		{
			resetChess();
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public void setChess( Int32 aKingPos, ModelItem aLastMovedItem, List< Int32 > aPossibleMoves )
		{
			isChess = true;
			kingPosition = aKingPos;
			chessGiverPosition = aLastMovedItem.index;

			switch ( aLastMovedItem.figureItem.figureType )
			{
			case FigureType.BISHOP:
			case FigureType.QUEEN:
			case FigureType.ROOK:
				{
					positionsToBreakChess = getPositionsToBreakChess( aKingPos, aLastMovedItem, aPossibleMoves );
				} break;
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public void resetChess()
		{
			kingPosition			= 0;
			chessGiverPosition		= 0;
			positionsToBreakChess	= new List< Int32 >();
			isChess					= false;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private List< Int32 > getPositionsToBreakChess( Int32 aKingPos, ModelItem aFigureToGiveChess, List< Int32 > aPossibleMoves )
		{
			List< Int32 > returnPositions = new List< Int32 >();

			Boolean isInSameRow			= ( aFigureToGiveChess.index / 8 ) == ( aKingPos / 8 );
			if( isInSameRow )
			{
				Int32 add = ( aFigureToGiveChess.index < aKingPos ? +1 : -1 );
				for ( Int32 index = ( aFigureToGiveChess.index + add ); index != aKingPos; index += add )
				{
					returnPositions.Add( index );
				}
				return returnPositions;
			}

			Boolean isInSameColumn		= ( aFigureToGiveChess.index % 8 ) == ( aKingPos % 8 );
			if( isInSameColumn )
			{
				Int32 add = ( aFigureToGiveChess.index < aKingPos ? +8 : -8 );
				for ( Int32 index = ( aFigureToGiveChess.index + add ); index != aKingPos; index += add )
				{
					returnPositions.Add( index );
				}
				return returnPositions;
			}
			Int32 kingX = aKingPos / 8;
			Int32 kingY = aKingPos % 8;

			Boolean isInLeftToRightDiagonal		= ( aFigureToGiveChess.x - kingX ) == ( aFigureToGiveChess.y - kingY );
			if( isInLeftToRightDiagonal )
			{
				Int32 add = ( aFigureToGiveChess.index < aKingPos ? +9 : -9 );
				for ( Int32 index = ( aFigureToGiveChess.index + add ); index != aKingPos; index += add )
				{
					returnPositions.Add( index );
				}
				return returnPositions;
			}

			Boolean isInRightToLeftDiagonal		= ( aFigureToGiveChess.x - kingX ) == ( -1 * ( aFigureToGiveChess.y - kingY ) );
			if( isInRightToLeftDiagonal )
			{
				Int32 add = ( aFigureToGiveChess.index < aKingPos ? +7 : -7 );
				for ( Int32 index = ( aFigureToGiveChess.index + add ); index != aKingPos; index += add )
				{
					returnPositions.Add( index );
				}
				return returnPositions;
			}
			return returnPositions;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public Int32 kingPosition{ get; set; }
		public Int32 chessGiverPosition{ get; set; }
		public List< Int32 > positionsToBreakChess{ get; set; }
		public Boolean isChess { get; set; }
	}
}
