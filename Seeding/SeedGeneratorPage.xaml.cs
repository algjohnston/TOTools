namespace CS341Project.Seeding;

public partial class SeedGeneratorPage : ContentPage
{
    public SeedGeneratorPage()
    {
        InitializeComponent();
    }

    private void SubmitButton_OnClicked(object? sender, EventArgs e)
    {
        Shell.Current.GoToAsync("double_elim_page");
    }

    private void ManualEntryButton_OnClicked(object? sender, EventArgs e)
    {
        Shell.Current.GoToAsync("select_competitors_page");
    }
}