using TOTools.AppEntry;
using TOTools.EventMap;
using TOTools.Scheduler;
using TOTools.Seeding;

namespace TOTools;

public partial class App : Application
{
    public App(
        EventBusinessLogic eventBusinessLogic,
        SchedulerBusinessLogic schedulerBusinessLogic,
        SeedingBusinessLogic seedingBusinessLogic)
    {
        InitializeComponent();

        // TODO the try catch is for testing the rest of the functionality later
        // since crash messages are not propagated anywhere I can find. 
        try
        {
            Task.Run(
                () =>
                {
                    eventBusinessLogic.LoadEvents();
                    schedulerBusinessLogic.LoadPastMatches();
                    seedingBusinessLogic.LoadPlayers();
                });

            MainPage = new NavigationPage(new TitlePage());
        }
        catch (Exception exception)
        {
            MainPage?.DisplayAlert("Error", exception.StackTrace, "OK");
        }
    }
}