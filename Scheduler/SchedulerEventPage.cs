using System.Collections.ObjectModel;
using TOTools.Models;

namespace TOTools.Scheduler;

/// <summary>
/// A page with a list of events that are used to come up with a match schedule.
/// </summary>
public partial class SchedulerEventPage : ContentPage, IOnEventLinkSubmitted
{
    private SchedulerBusinessLogic? _schedulerBusinessLogic;
    
    public SchedulerEventPage()
    {
        InitializeComponent();
        HandlerChanged += OnHandlerChanged;
    }
    
    private void OnHandlerChanged(object? sender, EventArgs e)
    {
        _schedulerBusinessLogic ??= Handler?.MauiContext?.Services
            .GetService<SchedulerBusinessLogic>();
        BindingContext = _schedulerBusinessLogic;

        // For testing
        _schedulerBusinessLogic?.AddEvent(
            new EventLink(
                "tournament/between-2-lakes-67-a-madison-super-smash-bros-tournament/event/ultimate-singles",
                DateTime.Now,
                3)
        );
    }

    private void OnSubmitButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new MatchSchedulerPage());
    }

    private void OnAddLinkButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new EventLinkPopup(this));
    }

    public void OnEventLinkSubmitted(EventLink eventLink)
    {
        _schedulerBusinessLogic?.AddEvent(eventLink);
    }
    
}