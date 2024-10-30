namespace CS341Project.ThumbGen;

/// <summary>
/// A page that takes a startgg link and extracts the parameters for the script generation.
/// </summary>
public partial class ThumbGenPage : ContentPage {

    public ThumbGenPage ()
    {
        InitializeComponent();
    }

    private void OnManualEntryButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new ThumbGenManualPage());
    }

    private void OnSelectTemplateButtonClicked(object? sender, EventArgs e)
    {
        
    }

    private void OnOutputFolderButtonClicked(object? sender, EventArgs e)
    {
        
    }

    private void OnSubmitButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}
