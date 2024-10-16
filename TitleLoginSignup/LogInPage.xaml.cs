namespace CS341Project.TitleLoginSignup;

public partial class LogInPage : ContentPage
{
	public LogInPage()
	{
		InitializeComponent();
	}

	private void OnLogInButtonClicked(object? sender, EventArgs e)
	{
		Navigation.PushAsync(new HomePage());
	}

	private void OnCancelButtonClicked(object? sender, EventArgs e)
	{
		Navigation.PushAsync(new TitleLoginSignup.TitlePage());
	}

}