using ChessTable.Common;
using System;
using System.Xml;

namespace ChessTable.Model
{
	public class FigureValues
	{
		public FigureValues( Colors aMyColor, String aFilePath = "" )
		{
			mMyColor = aMyColor;
			mFilePath = aFilePath;
			defaultValues();

			if ( ! String.IsNullOrEmpty( mFilePath ) )
			{
				readValues();
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private void defaultValues()
		{
			pawnValue		= 1;
			knightValue		= 3;
			bishopValue		= 3;
			rookValue		= 5;
			queenValue		= 9;
			kingValue		= 10;
			checkmatValue	= 11;
			drawValue		= -11;

			whiteDepth		= 3;
			blackDepth		= 3;

			secondsBetweenMove = 1000;
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private void readValues()
		{
			XmlDocument doc = new XmlDocument();
			try
			{
				doc.Load( mFilePath );
			}
			catch( Exception )
			{
				return;
			}

			XmlNode node;
			try
			{
				node			= doc.DocumentElement.SelectSingleNode("/FigureValue/Pawn");
				pawnValue		= Int32.Parse( node.InnerText );

				node			= doc.DocumentElement.SelectSingleNode("/FigureValue/Knight");
				knightValue		= Int32.Parse( node.InnerText );

				node			= doc.DocumentElement.SelectSingleNode("/FigureValue/Bishop");
				bishopValue		= Int32.Parse( node.InnerText );

				node			= doc.DocumentElement.SelectSingleNode("/FigureValue/Rook");
				rookValue		= Int32.Parse( node.InnerText );

				node			= doc.DocumentElement.SelectSingleNode("/FigureValue/Queen");
				queenValue		= Int32.Parse( node.InnerText );

				node			= doc.DocumentElement.SelectSingleNode("/FigureValue/King");
				kingValue		= Int32.Parse( node.InnerText );

				node			= doc.DocumentElement.SelectSingleNode("/FigureValue/CheckMate");
				checkmatValue	= Int32.Parse( node.InnerText );

				node			= doc.DocumentElement.SelectSingleNode("/FigureValue/Draw");
				drawValue		= Int32.Parse( node.InnerText );

				node			= doc.DocumentElement.SelectSingleNode("/FigureValue/WhiteDepth");
				whiteDepth		= Int32.Parse( node.InnerText );

				node			= doc.DocumentElement.SelectSingleNode("/FigureValue/BlackDepth");
				blackDepth		= Int32.Parse( node.InnerText );

				node			= doc.DocumentElement.SelectSingleNode("/FigureValue/SecondsBetweenMove");
				secondsBetweenMove		= ( Int32.Parse( node.InnerText ) * 1000 );
			} 
			catch( Exception )
			{
				return;
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public Int32 pawnValue { get; private set; }
		public Int32 knightValue { get; private set; }
		public Int32 bishopValue { get; private set; }
		public Int32 rookValue { get; private set; }
		public Int32 queenValue { get; private set; }
		public Int32 kingValue { get; private set; }
		public Int32 checkmatValue { get; private set; }
		public Int32 drawValue { get; private set; }

		public Int32 whiteDepth { get; private set; }
		public Int32 blackDepth { get; private set; }

		public Int32 secondsBetweenMove { get; private set; }

		//----------------------------------------------------------------------------------------------------------------------------------------

		private String mFilePath;
		private Colors mMyColor;
	}
}
