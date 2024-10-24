namespace CS341Project.AppEntry;

public partial class SignUpPage : ContentPage {

    public SignUpPage () {
        InitializeComponent();
    }
    
    private void OnOkButtonClicked(object? sender, EventArgs e)
    {
        Shell.Current.GoToAsync("..");
    }
    
}