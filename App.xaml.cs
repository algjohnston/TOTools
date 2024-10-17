using CS341Project.TitleLoginSignup;

namespace CS341Project;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new NavigationPage(new TitlePage());
    }
}