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
        public DelegateCommand onMenuStartBtnClickedCommand { get; private set; }
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
                CustomBoardViewModel customBoardViewModel = new CustomBoardViewModel( selectedColor );
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
            mPlayers.Add( "Player one" );
            mPlayers.Add( "Player two" );

            selectedPlayer = Player.PLAYER_ONE;

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

        public Player selectedPlayer
        {
            get
            {
                return mSelectedPlayer;
            }
            set
            {
                if ( value != mSelectedPlayer )
                {
                    mSelectedPlayer = value;
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

        public ObservableCollection<String> mGameTypes { get; set; }
        private GameType mSelectedGameType;
        public ObservableCollection<String> mColors { get; set; }
        private Colors mSelectedColor;
        public ObservableCollection<String> mPlayers { get; set; }
        private Player mSelectedPlayer;
        public ObservableCollection<String> mPlayerAlgorithms { get; set; }
        private Algorithm mPlayerOneAlgorithm;
        private Algorithm mPlayerTwoAlgorithm;

        private ChessBoardView mChessBoardView;
        private ChessBoardViewModel mChessBoardViewModel;

        private ChessBoardModel mChessBoardModel;        
    }
}
