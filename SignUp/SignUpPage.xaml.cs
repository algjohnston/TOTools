<<<<<<<< HEAD:SignUp/SignUpPage.xaml.cs
namespace CS341Project.SignUp;

public partial class SignUpPage : ContentPage {

    public SignUpPage () {
        InitializeComponent();
    }


    private void OnOkButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    
========
namespace CS341Project.TitleLoginSignup;

public partial class SignUpPage : ContentPage {

    public SignUpPage () {
        InitializeComponent();
    }


    private void OnOkButtonClicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    
>>>>>>>> origin/master:TitleLoginSignup/SignUpPage.xaml.cs
}