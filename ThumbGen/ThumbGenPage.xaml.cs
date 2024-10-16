namespace CS341Project.Thumb_Gen;
using Microsoft.Win32;
using System.Windows;

public partial class ThumbGenPage : ContentPage {

    public ThumbGenPage ()
    {
        InitializeComponent();
    }


    private void OnBackButtonClicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnManualEntryButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new ThumbGenManualPage());
    }

    private void OnSelectTemplateButtonClicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnOutputFolderButtonClicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnSubmitButtonClicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}
