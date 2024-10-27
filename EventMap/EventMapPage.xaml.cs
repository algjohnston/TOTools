namespace CS341Project.EventMap;

public partial class EventMapPage : ContentPage
{
	
	public EventMapPage()
	{
		// Initializes and displays map
		InitializeComponent();
	}

	private void ViewAllButton_OnClicked(object? sender, EventArgs e)
	{
		// TODO the try catch is for testing the rest of the functionality later
		// since crash messages are not propagated anywhere I can find. 
		try
		{
			Navigation.PushAsync(new EventListPage());
		}
		catch (Exception exception)
		{
			DisplayAlert("Error", exception.Message, "OK");
		}
	}
}