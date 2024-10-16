namespace CS341Project.SignUp;

public partial class SignUpPage : ContentPage {

    public SignUpPage () {
        InitializeComponent();
    }


    private void OnOkButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    
}