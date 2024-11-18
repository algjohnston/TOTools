using System.Collections.ObjectModel;
using GraphQL.Client.Http;
using TOTools.Models;

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
        InitializeComponent();
        HandlerChanged += OnHandlerChanged;
    }

    private void OnHandlerChanged(object? sender, EventArgs e)
    {
        _schedulerBusinessLogic ??= Handler?.MauiContext?.Services
            .GetService<SchedulerBusinessLogic>();
        _schedulerBusinessLogic?.LoadPotentialSchedule();
        MatchList.BindingContext = _schedulerBusinessLogic;
    }
    
}