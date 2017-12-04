using ChessTable.Common;
using ChessTable.ViewModels;
using System;
using System.Windows;

namespace ChessTable.View
{
	/// <summary>
	/// Interaction logic for ChooseFigure.xaml
	/// </summary>
	public partial class ChooseFigureView : Window
	{
		public ChooseFigureView()
		{
			InitializeComponent();

			ChooseFigureViewModel dataContext = new ChooseFigureViewModel();
			dataContext.closeChooseFigureView += new EventHandler< FigureType >( onCloseChooseFigureView );
			this.DataContext = dataContext;

		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		private void onCloseChooseFigureView( Object sender, FigureType aEventArgs )
		{
			selectedFigureType = aEventArgs;
			this.Close();
		}

		//----------------------------------------------------------------------------------------------------------------------------------------

		public FigureType selectedFigureType{ get; private set; }
	}
}
