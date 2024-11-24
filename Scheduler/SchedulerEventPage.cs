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
    
    private async void OnHandlerChanged(object? sender, EventArgs e)
    {
        _schedulerBusinessLogic ??= Handler?.MauiContext?.Services
            .GetService<SchedulerBusinessLogic>();
        if (_schedulerBusinessLogic == null)
        {
            return;
        }
        await _schedulerBusinessLogic.LoadTask;
        BindingContext = _schedulerBusinessLogic;
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