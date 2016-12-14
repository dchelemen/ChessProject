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
        private MenuView mainView;
        private MenuViewModel menuViewModel;

        App()
        {
            mainView = new MenuView();
            menuViewModel = new MenuViewModel();
            mainView.DataContext = menuViewModel;
            mainView.Show();
        }
    }
}
