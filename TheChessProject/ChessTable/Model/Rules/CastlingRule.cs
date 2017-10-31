using ChessTable.Common;
using System;
using System.Collections.Generic;

namespace ChessTable.Model.Rules
{
	public class CastlingRule
	{
		public CastlingRule()
		{
			isWhiteKingNotMoved			= true;
			isWhiteLeftRookNotMoved		= true;
			isWhiteRightRookNotMoved	= true;

			isBlackKingNotMoved			= true;
			isBlackLeftRookNotMoved		= true;
			isBlackRightRookNotMoved	= true;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public Boolean isCastling( ModelItem aFigureToMove, ModelItem aPlaceHere )
		{
			if ( aFigureToMove.figureItem.figureType != FigureType.KING )
			{
				return false;
			}

			if ( Math.Abs( aFigureToMove.y - aPlaceHere.y ) == 2 ) // do we steps 2 squares?
			{
				return true;
			}

			return false;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public Boolean canCastling( ModelItem aFigureToMove, List< List< ModelItem > > aChessBoard, Int32 aX, Int32 aY )
		{
			if ( aFigureToMove.figureItem.color == Colors.WHITE && ! isWhiteKingNotMoved || aFigureToMove.figureItem.color == Colors.BLACK && ! isBlackKingNotMoved )
			{
				return false;
			}

			Int32 stepMore					= ( aY > 0 ? aY + 1 : aY - 1 );
			FigureType theFigureWeCheck		= aChessBoard[ aFigureToMove.x ][ aFigureToMove.y + stepMore ].figureItem.figureType;
			Boolean isTheWayEmpty			= theFigureWeCheck == FigureType.ROOK || theFigureWeCheck == FigureType.NO_FIGURE;
			Boolean isTheCurrentRookNotMoved;

			if ( aFigureToMove.figureItem.color == Colors.WHITE )
			{
				isTheCurrentRookNotMoved = ( aY > 0 && isWhiteRightRookNotMoved || aY < 0 && isWhiteLeftRookNotMoved );
				if ( isTheCurrentRookNotMoved && isTheWayEmpty )
				{
					return true;
				}
				return false;
			}

			isTheCurrentRookNotMoved = ( aY > 0 && isBlackRightRookNotMoved || aY < 0 && isBlackLeftRookNotMoved );
			if ( isTheCurrentRookNotMoved && isTheWayEmpty )
			{
				return true;
			}
			return false;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public void updateCastlingState( ModelItem mFigureToMove )
		{
			if ( mFigureToMove.figureItem.figureType != FigureType.KING && mFigureToMove.figureItem.figureType != FigureType.ROOK )
			{
				return;
			}

			if ( mFigureToMove.figureItem.color == Colors.WHITE )
			{
				if ( mFigureToMove.figureItem.figureType == FigureType.KING )
				{
					isWhiteKingNotMoved = false;
					return;
				}
				if ( mFigureToMove.y == 0 )
				{
					isWhiteLeftRookNotMoved = false;
					return;
				}
				if ( mFigureToMove.y == 7 )
				{
					isWhiteRightRookNotMoved = false;
					return;
				}
			}
			
			if ( mFigureToMove.figureItem.figureType == FigureType.KING )
			{
				isBlackKingNotMoved = false;
				return;
			}
			if ( mFigureToMove.y == 0 )
			{
				isBlackLeftRookNotMoved = false;
				return;
			}
			if ( mFigureToMove.y == 7 )
			{
				isBlackRightRookNotMoved = false;
				return;
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public void disable()
		{
			isWhiteKingNotMoved			= false;
			isWhiteLeftRookNotMoved		= false;
			isWhiteRightRookNotMoved	= false;

			isBlackKingNotMoved			= false;
			isBlackLeftRookNotMoved		= false;
			isBlackRightRookNotMoved	= false;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private Boolean isWhiteKingNotMoved;
		private Boolean isWhiteLeftRookNotMoved;
		private Boolean isWhiteRightRookNotMoved;

		private Boolean isBlackKingNotMoved;
		private Boolean isBlackLeftRookNotMoved;
		private Boolean isBlackRightRookNotMoved;
	}
}
