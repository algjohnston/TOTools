namespace CS341Project.AppEntry;

public partial class HomePage : ContentPage {

    public HomePage () {
        InitializeComponent();
    }
    
    private void OnLogOutButtonClicked(object? sender, EventArgs e)
    {
        Shell.Current.GoToAsync("..");
    }

    private void OnSeedingButtonClicked(object? sender, EventArgs e)
    {
        Shell.Current.GoToAsync("SeedGeneratorPage");
    }
    
    private void OnSchedulerButtonClicked(object? sender, EventArgs e)
    {
        Shell.Current.GoToAsync("scheduler_event_page");
    }

    private void OnEventMapButtonClicked(object? sender, EventArgs e)
    {
        Shell.Current.GoToAsync("event_map_page");
    }

    private void OnThumbnailGeneratorButtonClicked(object? sender, EventArgs e)
    {
        Shell.Current.GoToAsync("thumbnail_gen_page");
    }
    
}