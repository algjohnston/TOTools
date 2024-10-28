namespace CS341Project.Seeding;

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