using TOTools.EventMap;

namespace TOTools.AppEntry;

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

    [Obsolete]
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Remove padding programmatically if necessary
        if (Device.RuntimePlatform == Device.Android)
        {
            this.Padding = new Thickness(0); // Removes extra padding
        }
    }


}
