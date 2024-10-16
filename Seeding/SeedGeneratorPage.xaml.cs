namespace CS341Project.Seeding;

public partial class SeedGeneratorPage : ContentPage
{
    public SeedGeneratorPage()
    {
        InitializeComponent();
    }

    private void SubmitButton_OnClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new DoubleElimPage());
    }
}