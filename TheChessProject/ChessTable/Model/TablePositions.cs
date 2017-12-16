using ChessTable.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ChessTable.Model
{
	public class TablePositions
	{
		public TablePositions()
		{
			mFilePath = "TablePositions.xml";
			readValues();
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public void addNewPosition( String name, String Value )
		{
			XmlDocument doc = new XmlDocument();
			if ( ! File.Exists( mFilePath ) )
			{
				doc.AppendChild(doc.CreateElement("TablePositions"));
				doc.Save( mFilePath );
			}
			else
			{
				doc.Load( mFilePath );
			}
			
			XmlNode el = doc.FirstChild;
			XmlNode position = el.AppendChild( doc.CreateElement( "Position" ) );
			position.AppendChild( doc.CreateElement( "Name" ) ).InnerText = name;
			position.AppendChild( doc.CreateElement( "Value" ) ).InnerText = Value;
			doc.Save( mFilePath );
		}

		private void readValues()
		{
			tablePositions	= new List< Positions > ();
			saveNames		= new List< String >();
			if ( ! File.Exists( mFilePath ) )
			{
				return;
			}

			XmlDocument doc = new XmlDocument();
			doc.Load( mFilePath );
			XmlNodeList positions = doc.DocumentElement.SelectNodes("/TablePositions/Position");
			foreach ( XmlNode position in positions )
			{
				Positions p = new Positions();
				p.name = position.FirstChild.InnerText;
				p.value = position.LastChild.InnerText;
				tablePositions.Add( p );
				saveNames.Add( p.name );
			}
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public List< Positions >			tablePositions;
		public List< String >				saveNames;

		//----------------------------------------------------------------------------------------------------------------------------------------

		private String mFilePath;
	}
}
