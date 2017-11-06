using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTable.Model
{
	public class Move
	{
		public Move(){}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public Move( Int32 aFromIndex, Int32 aToIndex )
		{
			mMoveFromIndex	= aFromIndex;
			mMoveToIndex	= aToIndex;

			mMoveFromX		= ( aFromIndex % 8 );
			mMoveFromY		= ( aFromIndex / 8 );

			mMoveToX		= ( aToIndex % 8 );
			mMoveToY		= ( aToIndex / 8 );
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public Move( Int32 aFromX, Int32 aFromY, Int32 aToX, Int32 aToY )
		{
			mMoveFromIndex	= ( ( aFromX * 8 ) + aFromY );
			mMoveToIndex	= ( ( aToX * 8 ) + aToY );;

			mMoveFromX		= aFromX;
			mMoveFromY		= aFromY;

			mMoveToX		= aToX;
			mMoveToY		= aToY;
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public Int32 mMoveFromIndex;
		public Int32 mMoveFromX;
		public Int32 mMoveFromY;

		public Int32 mMoveToIndex;
		public Int32 mMoveToX;
		public Int32 mMoveToY;
	}
}
