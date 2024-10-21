using CS341Project.AppEntry;
using Microsoft.Maui.Controls;

namespace CS341Project;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new NavigationPage(new TitlePage());
    }
    
}