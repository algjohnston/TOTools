namespace TOTools.Seeding;

/// <summary>
/// A page that takes in a list of players and produces a double elimination bracket.
/// </summary>
public partial class SeedGeneratorPage : ContentPage
{
    public SeedGeneratorPage()
    {
        InitializeComponent();
    }

    private void OnSubmitButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new DoubleElimPage());
    }

    private void OnManualEntryButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new SelectCompetitorsPage());
    }
}