using CS341Project.Scheduler;
using CS341Project.Seeding;
using CS341Project.ThumbGen;

namespace CS341Project;

public partial class HomePage : ContentPage {

    public HomePage () {
        InitializeComponent();
    }
    
    private void OnLogOutButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private void OnSeedingButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new SeedingShell());
    }
    
    private void OnSchedulerButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new MatchSchedulerPage());
        // TODO waiting for SchedulerEventPage
    }

    private void OnEventMapButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new EventMap.EventMapPage());
    }

    private void OnThumbnailGeneratorButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new ThumbGenPage());
    }
    
}