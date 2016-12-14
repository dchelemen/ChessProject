using System;
using ChessTable.View;
using ChessTable.ViewModels.ImplementedInterfaces;
using System.Collections.ObjectModel;
using ChessTable.Model;
using ChessTable.Common;

namespace ChessTable.ViewModels
{
    public class MenuViewModel : ViewModelBase
    { 
        public MenuViewModel()
        {
            mGameTypes          = new ObservableCollection< String >();
            mColors             = new ObservableCollection< String >();
            mPlayers            = new ObservableCollection< String >();
            mPlayerAlgorithms   = new ObservableCollection< String >();

            onMenuStartBtnClickedCommand = new DelegateCommand( x => onMenuStartBtnClicked() );
            setupComboBoxes();
        }
        public void onMenuStartBtnClicked()
        {
            if ( mSelectedGameType == GameType.CUSTOM_GAME )
            {
                CustomBoardView customBoardView = new CustomBoardView();
                CustomBoardViewModel customBoardViewModel = new CustomBoardViewModel( selectedColor, selectedStartingColor, playerOneAlgorithm, playerTwoAlgorithm );
                customBoardView.DataContext = customBoardViewModel;
                customBoardView.ShowDialog();
                mChessBoardModel = customBoardViewModel.chessBoardModel;
            }
            else if ( mSelectedGameType == GameType.END_GAME )
            {

            }
            else if ( mSelectedGameType == GameType.STANDARD_GAME )
            {

            }

            mChessBoardView = new ChessBoardView();
            mChessBoardViewModel = new ChessBoardViewModel( mChessBoardModel );
            mChessBoardView.DataContext = mChessBoardViewModel;
            mChessBoardView.ShowDialog();
        }

        private void setupComboBoxes()
        {
            mGameTypes.Clear();
            mGameTypes.Add( "Standard Game" );
            mGameTypes.Add( "End Game"      );
            mGameTypes.Add( "Custom Game"   );

            selectedGameType = GameType.STANDARD_GAME;

            mColors.Clear();
            mColors.Add( "White" );
            mColors.Add( "Black" );

            selectedColor = Colors.WHITE;

            mPlayers.Clear();
            mPlayers.Add( "White" );
            mPlayers.Add( "Black" );

            selectedStartingColor = Colors.WHITE;

            mPlayerAlgorithms.Clear();
            mPlayerAlgorithms.Add( "Human" );
            mPlayerAlgorithms.Add( "Random" );
            mPlayerAlgorithms.Add( "Alpha-Beta" );

            playerOneAlgorithm = Algorithm.HUMAN;
            playerTwoAlgorithm = Algorithm.HUMAN;
        }

        public GameType selectedGameType
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

        public Colors selectedColor
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

        public Colors selectedStartingColor
        {
            get
            {
                return mSelectedStartingColor;
            }
            set
            {
                if ( value != mSelectedStartingColor )
                {
                    mSelectedStartingColor = value;
                    OnPropertyChanged( "selectedPlayer" );
                }
            }
        }

        public Algorithm playerOneAlgorithm
        {
            get
            {
                return mPlayerOneAlgorithm;
            }
            set
            {
                if ( value != mPlayerOneAlgorithm )
                {
                    mPlayerOneAlgorithm = value;
                    OnPropertyChanged( "selectedPlayerOneAlgorithm" );
                }
            }
        }

        public Algorithm playerTwoAlgorithm
        {
            get
            {
                return mPlayerTwoAlgorithm;
            }
            set
            {
                if ( value != mPlayerTwoAlgorithm )
                {
                    mPlayerTwoAlgorithm = value;
                    OnPropertyChanged( "selectedPlayerTwoAlgorithm" );
                }
            }
        }

        public DelegateCommand onMenuStartBtnClickedCommand { get; private set; }

        public ObservableCollection<String> mGameTypes { get; set; }
        private GameType mSelectedGameType;
        public ObservableCollection<String> mColors { get; set; }
        private Colors mSelectedColor;
        public ObservableCollection<String> mPlayers { get; set; }
        private Colors mSelectedStartingColor;
        public ObservableCollection<String> mPlayerAlgorithms { get; set; }
        private Algorithm mPlayerOneAlgorithm;
        private Algorithm mPlayerTwoAlgorithm;

        private ChessBoardView mChessBoardView;
        private ChessBoardViewModel mChessBoardViewModel;

        private ChessBoardModel mChessBoardModel;        
    }
}
