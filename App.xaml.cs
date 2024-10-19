using CS341Project.AppEntry;

namespace CS341Project;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new NavigationPage(new TitlePage());
    }
}