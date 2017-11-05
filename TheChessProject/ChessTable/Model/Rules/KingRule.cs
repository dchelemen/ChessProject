using ChessTable.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessTable.Model.Rules
{
	class KingRule : RulesBase
	{
		
		//----------------------------------------------------------------------------------------------------------------------------------------

		public KingRule( List< List< ModelItem > > aChessBoard, Colors aPlayer1Color, ModelItem aFigureToMove, List< ModelItem > aBlackFigures, List< ModelItem > aWhiteFigures, CastlingRule aCastlingRule = null )
			: base( aChessBoard, aPlayer1Color, aFigureToMove )
		{
			mBlackFigures = aBlackFigures;
			mWhiteFigures = aWhiteFigures;
			mCastlingRule = aCastlingRule;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public override List< Int32 > possibleMoves()
		{
			mPossibleMoves = new List< Int32 >();

			setOnePossibleMove( +1, -1 ); // Lets move Down and Left;
			setOnePossibleMove( +1, +0 ); // Lets move Down;
			setOnePossibleMove( +1, +1 ); // Lets move Down and Right;
			setOnePossibleMove( +0, +1 ); // Lets move Right;
			setOnePossibleMove( -1, +1 ); // Lets move Up and Right;
			setOnePossibleMove( -1, +0 ); // Lets move Up;
			setOnePossibleMove( -1, -1 ); // Lets move Up and Left;
			setOnePossibleMove( +0, -1 ); // Lets move Left;

			Boolean canCastling		= mCastlingRule.canCastling( mFigureToMove, mChessBoard, +0, -2 );
			Boolean isTheWayClear	= mPossibleMoves.Contains( mFigureToMove.index - 1 );
			if ( canCastling && isTheWayClear )
			{
				setOnePossibleMove( +0, -2 );
			}

			canCastling		= mCastlingRule.canCastling( mFigureToMove, mChessBoard, +0, +2 );
			isTheWayClear	= mPossibleMoves.Contains( mFigureToMove.index + 1 );
			if ( canCastling && isTheWayClear )
			{
				setOnePossibleMove( +0, +2 );
			}
			return mPossibleMoves;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public List< Int32 > possibleMovesForDeepLvl()
		{
			mPossibleMoves = new List< Int32 >();

			base.setOnePossibleMove( +1, -1 ); // Lets move Down and Left;
			base.setOnePossibleMove( +1, +0 ); // Lets move Down;
			base.setOnePossibleMove( +1, +1 ); // Lets move Down and Right;
			base.setOnePossibleMove( +0, +1 ); // Lets move Right;
			base.setOnePossibleMove( -1, +1 ); // Lets move Up and Right;
			base.setOnePossibleMove( -1, +0 ); // Lets move Up;
			base.setOnePossibleMove( -1, -1 ); // Lets move Up and Left;
			base.setOnePossibleMove( +0, -1 ); // Lets move Left;

			return mPossibleMoves;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		protected override void setOnePossibleMove( int aAddX, int aAddY )
		{
			Int32 xCoord						= mFigureToMove.x + aAddX;
			Int32 yCoord						= mFigureToMove.y + aAddY;

			if ( ! isValidField( xCoord, yCoord ) )
			{
				return;
			}

			ModelItem modelItem = mChessBoard[ xCoord ][ yCoord ];

			List< ModelItem > enemyFigures;
			if ( mFigureToMove.figureItem.color == Colors.WHITE )
			{
				enemyFigures = mBlackFigures;
			}
			else
			{
				enemyFigures = mWhiteFigures;
			}

			ModelItem targetFigure = new ModelItem( modelItem.x, modelItem.y, modelItem.figureItem.color, modelItem.figureItem.figureType );
			modelItem.figureItem.figureType		= mFigureToMove.figureItem.figureType;
			modelItem.figureItem.color			= mFigureToMove.figureItem.color;

			mChessBoard[ mFigureToMove.x ][ mFigureToMove.y ].figureItem.figureType		= FigureType.NO_FIGURE;
			mChessBoard[ mFigureToMove.x ][ mFigureToMove.y ].figureItem.color			= Colors.NO_COLOR;


			foreach ( ModelItem enemy in enemyFigures )
			{
				List< Int32 > enemysPossibleMove = possibleMoveOfEnemy( enemy );
				if ( enemysPossibleMove.Contains( modelItem.index ) )
				{
					modelItem.figureItem.figureType		= targetFigure.figureItem.figureType;
					modelItem.figureItem.color			= targetFigure.figureItem.color;

					mChessBoard[ mFigureToMove.x ][ mFigureToMove.y ].figureItem.figureType		= mFigureToMove.figureItem.figureType;
					mChessBoard[ mFigureToMove.x ][ mFigureToMove.y ].figureItem.color			= mFigureToMove.figureItem.color;
					return;
				}
			}

			modelItem.figureItem.figureType		= targetFigure.figureItem.figureType;
			modelItem.figureItem.color			= targetFigure.figureItem.color;

			mChessBoard[ mFigureToMove.x ][ mFigureToMove.y ].figureItem.figureType		= mFigureToMove.figureItem.figureType;
			mChessBoard[ mFigureToMove.x ][ mFigureToMove.y ].figureItem.color			= mFigureToMove.figureItem.color;

			mPossibleMoves.Add( ( ( 8 * xCoord ) + yCoord ) );
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		List< Int32 > possibleMoveOfEnemy( ModelItem aEnemy )
		{
			List< Int32 > possibleMoves = new List< Int32 >();
			switch ( aEnemy.figureItem.figureType )
			{
			case FigureType.KING:
				{
					KingRule kingRule		= new KingRule( mChessBoard, mPlayer1Color, aEnemy, mBlackFigures, mWhiteFigures );
					possibleMoves			= kingRule.possibleMovesForDeepLvl();
				} break;
			case FigureType.QUEEN:
				{
					QueenRule queenRule		= new QueenRule( mChessBoard, mPlayer1Color, aEnemy );
					possibleMoves			= queenRule.possibleMoves();
				} break;
			case FigureType.ROOK:
				{
					RookRule rookRule		= new RookRule( mChessBoard, mPlayer1Color, aEnemy );
					possibleMoves			= rookRule.possibleMoves();
				} break;
			case FigureType.BISHOP:
				{
					BishopRule bishopRule	= new BishopRule( mChessBoard, mPlayer1Color, aEnemy );
					possibleMoves			= bishopRule.possibleMoves();
				} break;
			case FigureType.KNIGHT:
				{
					KnightRule knightRule	= new KnightRule( mChessBoard, mPlayer1Color, aEnemy );
					possibleMoves			= knightRule.possibleMoves();
				} break;
			case FigureType.PAWN:
				{
					PawnRule pawnRule		= new PawnRule( mChessBoard, mPlayer1Color, aEnemy );
					possibleMoves			= pawnRule.possibleMoves();
				} break;
			case FigureType.NO_FIGURE:		break;
			}
			return possibleMoves;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		List< ModelItem > mBlackFigures;
		List< ModelItem > mWhiteFigures;
		CastlingRule mCastlingRule;
	}
}
