using ChessTable.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTable.Model.Rules
{
	class EnPassantRule
	{
		public EnPassantRule()
		{
			isEnPassantActive	= false;
			originalPawn		= null;
			temporaryPawn		= null;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private void reset()
		{
			if ( isEnPassantActive )
			{
				isEnPassantActive						= false;
				originalPawn							= null;

				temporaryPawn.figureItem.color			= Colors.NO_COLOR;
				temporaryPawn.figureItem.figureType		= FigureType.NO_FIGURE;
				temporaryPawn							= null;
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public void setEnPassant( ModelItem aPawn, ModelItem aMoveTo, List< List< ModelItem > > aChessBoardModel )
		{
			reset();

			if ( aPawn.figureItem.figureType != FigureType.PAWN )
			{
				return;
			}

			if ( Math.Abs( aPawn.x - aMoveTo.x ) != 2 )
			{
				return;
			}

			Int32 temporaryX					= ( aPawn.x == 6 ? 5 : 2 );
			originalPawn						= aChessBoardModel[ aMoveTo.x ][ aMoveTo.y ];
			temporaryPawn						= aChessBoardModel[ temporaryX ][ aPawn.y ];

			temporaryPawn.figureItem.color		= aPawn.figureItem.color;
			temporaryPawn.figureItem.figureType	= aPawn.figureItem.figureType;

			isEnPassantActive = true;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public Boolean isEnPassantActive { get; set; }

		//----------------------------------------------------------------------------------------------------------------------------------------

		public ModelItem originalPawn { get; set; }
		public ModelItem temporaryPawn { get; set; }
	}
}
