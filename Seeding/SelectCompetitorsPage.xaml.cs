using System.Collections.ObjectModel;

namespace TOTools.Seeding;

/// <summary>
/// A page that allows the user to select a list of competitors to be used in bracket generation.
/// </summary>
public partial class SelectCompetitorsPage : ContentPage
{

	public ObservableCollection<string> Competitors { get; } = [];
	
	public SelectCompetitorsPage()
	{
		InitializeComponent();
		BindingContext = this;
		
		Competitors.Add("Player 1");
		Competitors.Add("Player 2");
		Competitors.Add("Player 3");
		Competitors.Add("Player 4");
		Competitors.Add("Player 5");
	}

	private void OnSubmitClicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new DoubleElimPage());
	}
}
