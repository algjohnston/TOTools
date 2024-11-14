using System.Collections.ObjectModel;
using GraphQL.Client.Http;
using TOTools.Models;
using TOTools.StartGGAPI;

namespace TOTools.Scheduler;

/// <summary>
/// A page that displays a potential match schedule
/// for all matches in a given tournament.
/// </summary>
public partial class MatchSchedulerPage : ContentPage
{

    private SchedulerBusinessLogic? _schedulerBusinessLogic;

    // TODO This is a quick hack due to time constraints
    private ObservableCollection<EventLink> _events;

    public MatchSchedulerPage(ObservableCollection<EventLink> events)
    {
        _events = events;
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
        _schedulerBusinessLogic.LoadPotentialSchedule(_events);
        
        InitializeComponent();
        MatchList.BindingContext = _schedulerBusinessLogic;
    }
    
}