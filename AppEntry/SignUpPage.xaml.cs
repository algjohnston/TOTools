namespace TOTools.AppEntry;

/// <summary>
/// The sign-up page of the app.
/// </summary>
public partial class SignUpPage : ContentPage
{
    public SignUpPage()
    {
        InitializeComponent();
    }

    private void OnSubmitButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}