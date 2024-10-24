using CS341Project.EventMap;

namespace CS341Project.AppEntry;

public partial class TitlePage : ContentPage
{
	public TitlePage()
	{
		InitializeComponent();
	}

	private void OnLogInButtonClicked(object? sender, EventArgs e)
	{
		Shell.Current.GoToAsync("login_page");
	}

	private void OnSignUpButtonClicked(object? sender, EventArgs e)
	{
		Shell.Current.GoToAsync("sign_up_page");
	}

	private void OnSkipButtonClicked(object? sender, EventArgs e)
	{
		Shell.Current.GoToAsync("event_map_page");
	}

}
