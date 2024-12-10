using TOTools.EventMap;
using TOTools.Scheduler;
using TOTools.Seeding;

namespace TOTools.AppEntry;

/// <summary>
/// The home page of the app.
/// </summary>
public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    private void OnLogOutButtonClicked(object? sender, EventArgs e)
    {
        // Removes the log-in page so pop goes to the home screen
        // The stack is currently TitlePage,LogInPage,HomePage
        Navigation.RemovePage(Navigation.NavigationStack[^2]);
        Navigation.PopAsync();
    }

    private void OnSeedingButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new SeedingTabbedPage());
    }

    private void OnSchedulerButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new SchedulerEventPage());
    }

    private void OnEventMapButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new EventMapPage());
    }
}