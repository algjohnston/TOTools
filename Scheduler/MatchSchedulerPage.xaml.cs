using GraphQL.Client.Http;

namespace TOTools.Scheduler;

/// <summary>
/// A page that displays a potential match schedule
/// for all matches in a given tournament.
/// </summary>
public partial class MatchSchedulerPage : ContentPage
{

    private SchedulerBusinessLogic? _schedulerBusinessLogic;

    public MatchSchedulerPage()
    {
        HandlerChanged += OnHandlerChanged;
    }

    void OnHandlerChanged(object? sender, EventArgs e)
    {
        var client = Handler?.MauiContext?.Services.GetService<GraphQLHttpClient>();
        if (client == null)
        {
            DisplayAlert("Error", "There was an error trying to connect to start.gg", "OK");
            return;
        }
        
        _schedulerBusinessLogic = new SchedulerBusinessLogic(client);
        var task = _schedulerBusinessLogic.LoadPotentialMatchList(
            "tournament/between-2-lakes-67-a-madison-super-smash-bros-tournament/event/ultimate-singles");
        InitializeComponent();
        MatchList.BindingContext = _schedulerBusinessLogic;
    }
    
}