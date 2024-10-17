namespace CS341Project.ThumbGen;

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
