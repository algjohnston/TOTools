using TOTools.EventMap;

namespace TOTools.AppEntry;

/// <summary>
/// The title page of the app.
/// </summary>
public partial class TitlePage : ContentPage
{
    public TitlePage()
    {
        InitializeComponent();
    }

    private void OnLogInButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new LogInPage());
    }

    private void OnSignUpButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new SignUpPage());
    }

    private void OnSkipButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new EventMapPage());
    }
}