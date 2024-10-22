namespace CS341Project;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
        Shell.Current.GoToAsync("//title_page");
    }
    
}