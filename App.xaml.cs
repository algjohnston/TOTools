using CommunityToolkit.Mvvm.Messaging;
using CS341Project.AppEntry;

namespace CS341Project;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new NavigationPage(new TitlePage());
        
        // The BackPressedMessage is only sent by the SeedingShell and
        // is only used when the SeedingShell's navigation stack is empty 
        WeakReferenceMessenger.Default.Register<BackPressedMessage>(
            MainPage,
            (r, message) =>
            {
                    MainPage.Navigation.PopAsync();
            });
        

    }
    
}