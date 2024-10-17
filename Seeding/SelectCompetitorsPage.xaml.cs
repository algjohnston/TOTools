using System.Collections.ObjectModel;

namespace CS341Project.Seeding;

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
