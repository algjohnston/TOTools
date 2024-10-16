

namespace CS341Project;

public partial class HomePage : ContentPage {

    public HomePage () {
        InitializeComponent();
    }


    private void OnLogOutButtonClicked(object? sender, EventArgs e)
    {
		Navigation.PushAsync(new TitleLoginSignup.TitlePage());
    }

    private void OnSeedingButtonClicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
    
    private void OnSchedulerButtonClicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnEventMapButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new EventMap.EventMapPage());
    }

    private void OnThumbnailGeneratorButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new Thumb_Gen.ThumbGenPage());
    }
    
}