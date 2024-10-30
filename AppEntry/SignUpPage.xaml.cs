namespace TOTools.AppEntry;

public partial class SignUpPage : ContentPage {

    public SignUpPage () {
        InitializeComponent();
    }
    
    private void OnSubmitButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
    
}