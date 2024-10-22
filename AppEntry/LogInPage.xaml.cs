namespace CS341Project.AppEntry;

public partial class LogInPage : ContentPage
{
	public LogInPage()
	{
		InitializeComponent();
	}

	private void OnLogInButtonClicked(object? sender, EventArgs e)
	{
		Shell.Current.GoToAsync("home_page");
	}

	private void OnCancelButtonClicked(object? sender, EventArgs e)
	{
		Shell.Current.GoToAsync("title_page");
	}

}