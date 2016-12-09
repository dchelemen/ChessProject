using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessTable.View;
using ChessTable.ViewModels.ImplementedInterfaces;
using System.Windows;

namespace ChessTable.ViewModels
{
    public class MenuViewModel
    {
        private ChessBoardView chessBoardView;
        private CustomBoardView customBoardView;
        private CustomBoardViewModel mCustomBoardViewModel;
        public DelegateCommand onMenuStartBtnClickedCommand { get; private set; }
        public MenuViewModel()
        {
            onMenuStartBtnClickedCommand = new DelegateCommand( x => onMenuStartBtnClicked() );
        }
        public void onMenuStartBtnClicked()
        {
            customBoardView = new CustomBoardView();
            mCustomBoardViewModel = new CustomBoardViewModel();
            customBoardView.DataContext = mCustomBoardViewModel;
            customBoardView.Show();
        }
    }
}
