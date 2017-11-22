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

		public Move( ModelItem aItemFrom, ModelItem aItemTo )
		{
			itemFrom	= new ModelItem( aItemFrom.x, aItemFrom.y, aItemFrom.figureItem.color, aItemFrom.figureItem.figureType );
			itemTo		= new ModelItem( aItemTo.x, aItemTo.y, aItemTo.figureItem.color, aItemTo.figureItem.figureType );
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		public ModelItem itemFrom;

		public ModelItem itemTo;
	}
}
