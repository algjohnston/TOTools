namespace CS341Project.Seeding;

public partial class SelectCompetitorsPage : ContentPage
{
	public SelectCompetitorsPage()
	{
		InitializeComponent();
	}

	private void OnSubmitClicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new DoubleElimPage());
	}
}
