using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChessTable.View
{
	/// <summary>
	/// Interaction logic for Save.xaml
	/// </summary>
	public partial class Save : Window
	{
		public Save()
		{
			InitializeComponent();
		}

		public string SaveName
		{
			get
			{
				return SaveNameTextBox.Text;
			}
			set
			{
				SaveNameTextBox.Text = value;
			}
		}

		private void onSaveButtonClicked( object sender, RoutedEventArgs e )
		{
			if ( SaveName == "" )
			{
				return;
			}
			DialogResult = true;
		}

		private void onCancelButtonClicked( object sender, RoutedEventArgs e )
		{
			DialogResult = false;
		}
	}
}
