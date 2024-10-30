using TOTools.EventMap;
using TOTools.Scheduler;
using TOTools.Seeding;
using TOTools.ThumbGen;

namespace TOTools.AppEntry;

public partial class HomePage : ContentPage {

    public HomePage () {
        InitializeComponent();
    }
    
    private void OnLogOutButtonClicked(object? sender, EventArgs e)
    {
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

    private void OnThumbnailGeneratorButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new ThumbGenPage());
    }

    
}