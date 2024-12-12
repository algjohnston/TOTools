namespace TOTools.ThumbGen;

/// <summary>
/// A form to allow the user to specify the parameters for the thumbnail script generation.
/// </summary>
public partial class ThumbGenManualPage : ContentPage
{
    public ThumbGenManualPage()
    {
        InitializeComponent();
    }

    private void OnSelectTemplateButtonClicked(object sender, EventArgs e) {
            
    }

    private void OnOutputFolderButtonClicked(object sender, EventArgs e)
    {
            
    }

    private void OnSubmitButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}