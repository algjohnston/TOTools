namespace CS341Project.ThumbGen;

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
        Shell.Current.GoToAsync("..");
    }
}