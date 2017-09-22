using System.Windows;
using ChessTable.View;
using ChessTable.ViewModels;

namespace ChessTable
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		App()
		{
			menuViewModel			= new MenuViewModel();
			mainView				= new MenuView();
			mainView.DataContext	= menuViewModel; 
			mainView.Show();
		}

		//-----------------------------------------------------------------------------------------------------------------------------------------

		private MenuView			mainView;
		private MenuViewModel		menuViewModel;

	}
}
