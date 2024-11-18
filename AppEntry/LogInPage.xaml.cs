namespace TOTools.AppEntry;

/// <summary>
/// The login page of the app
/// </summary>
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
		Navigation.PushAsync(new TitlePage());
	}

}