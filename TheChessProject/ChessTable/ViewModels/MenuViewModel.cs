﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessTable.View;
using System.Windows;

namespace ChessTable.ViewModels
{
    public class MenuViewModel
    {
        private ChessBoardView chessBoardView;
        private CustomBoardView customBoardView;
        public DelegateCommand onMenuStartBtnClickedCommand { get; private set; }
        public MenuViewModel()
        {
            onMenuStartBtnClickedCommand = new DelegateCommand( x => onMenuStartBtnClicked() );
        }
        public void onMenuStartBtnClicked()
        {
            try
            {
                customBoardView = new CustomBoardView();
                customBoardView.Show();
            }
            catch ( Exception ex )
            {
                MessageBox.Show( ex.Message );
            }
        }
    }
}
