using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ChessTable.View;

namespace ChessTable
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MenuView mainView;
        private ChessBoardView chessBoard;

        App()
        {
            mainView = new MenuView();
            mainView.Show();
        }
    }
}
