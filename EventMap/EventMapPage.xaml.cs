namespace CS341Project.EventMap;

public partial class EventMapPage : ContentPage
{
	
	public EventMapPage()
	{
		// Initializes and displays map
		InitializeComponent();
	}

	private void OnViewAllEventsButtonClicked(object? sender, EventArgs e)
	{
		
			Navigation.PushAsync(new EventListPage());

	}
}