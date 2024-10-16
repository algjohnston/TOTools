namespace CS341Project.TitleLoginSignup;

public partial class TitlePage : ContentPage
{
	public TitlePage()
	{
		InitializeComponent();
	}

	private void OnLogInButtonClicked(object? sender, EventArgs e)
	{
		Navigation.PushAsync(new TitleLoginSignup.LogInPage());
	}

	private void OnSignUpButtonClicked(object? sender, EventArgs e)
	{
		Navigation.PushAsync(new TitleLoginSignup.SignUpPage());
	}

	private void OnSkipButtonClicked(object? sender, EventArgs e)
	{
		Navigation.PushAsync(new EventMap.EventMapPage());
	}

}
