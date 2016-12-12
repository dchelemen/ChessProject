using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessTable.View;
using ChessTable.ViewModels.ImplementedInterfaces;
using System.Windows;
using System.Collections.ObjectModel;
using ChessTable.Model;

namespace ChessTable.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        public DelegateCommand onMenuStartBtnClickedCommand { get; private set; }
        public MenuViewModel()
        {
            mGameTypes = new ObservableCollection< string >();
            mColors = new ObservableCollection<string>();
            onMenuStartBtnClickedCommand = new DelegateCommand( x => onMenuStartBtnClicked() );
            setupComboBoxes();
        }
        public void onMenuStartBtnClicked()
        {
            mChessBoardModel = new ChessBoardModel();
            if ( mSelectedGameType == "Custom Game" )
            {
                CustomBoardView customBoardView = new CustomBoardView();
                CustomBoardViewModel customBoardViewModel = new CustomBoardViewModel();
                customBoardView.DataContext = customBoardViewModel;
                customBoardView.ShowDialog();
            }
            else if ( mSelectedGameType == "Standard Game" )
            {

            }
            else
            {

            }

            mChessBoardView = new ChessBoardView();
            mChessBoardViewModel = new ChessBoardViewModel();
            mChessBoardView.DataContext = mChessBoardViewModel;
            mChessBoardView.ShowDialog();
        }

        private void setupComboBoxes()
        {
            mGameTypes.Clear();
            mGameTypes.Add( "Standard Game" );
            mGameTypes.Add( "End Game"      );
            mGameTypes.Add( "Custom Game"   );

            selectedGameType = mGameTypes.FirstOrDefault();

            mColors.Clear();
            mColors.Add( "White" );
            mColors.Add( "Black" );

            selectedColor = mColors.FirstOrDefault();
        }

        public String selectedGameType
        {
            get
            {
                return mSelectedGameType;
            }
            set
            {
                if ( value != mSelectedGameType )
                {
                    mSelectedGameType = value;
                    OnPropertyChanged( "selectedGameType" );
                }
            }
        }

        public String selectedColor
        {
            get
            {
                return mSelectedColor;
            }
            set
            {
                if ( value != mSelectedColor )
                {
                    mSelectedColor = value;
                    OnPropertyChanged( "selectedColor" );
                }
            }
        }

        public ObservableCollection<String> mGameTypes { get; set; }
        private String mSelectedGameType;
        public ObservableCollection<String> mColors { get; set; }
        private String mSelectedColor;

        private ChessBoardView mChessBoardView;
        private ChessBoardViewModel mChessBoardViewModel;

        private ChessBoardModel mChessBoardModel;        
    }
}
