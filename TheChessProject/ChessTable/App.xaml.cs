using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
