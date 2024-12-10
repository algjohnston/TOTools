using TOTools.Models;

namespace TOTools.Scheduler;

/// <summary>
/// A page that lets the user enter startgg event links.
/// Any input links are used to come up with a match schedule.
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

        // Waits for the loading of the past matches that are need by the scheduler
        await _schedulerBusinessLogic.PastMatchLoadTask;
        BindingContext = _schedulerBusinessLogic;
    }

    private void OnSubmitButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new MatchSchedulerPage());
    }

    private void OnAddLinkButtonClicked(object? sender, EventArgs e)
    {
        // this is passed in so the EventLinkPopup can pass the link the user entered to OnEventLinkSubmitted
        Navigation.PushAsync(new EventLinkPage(this));
    }

    /// <summary>
    /// This is part of the interface used by EventLinkPopup to submit links entered by the user.
    /// </summary>
    /// <param name="eventLink"></param>
    public void OnEventLinkSubmitted(EventLink eventLink)
    {
        _schedulerBusinessLogic?.AddEventLink(eventLink);
    }

    private void OnDeleteLinkButtonClicked(object? sender, EventArgs e)
    {
        _schedulerBusinessLogic?.RemoveEvent(_schedulerBusinessLogic.SelectedEventLink);
    }

    private void OnEventLinkSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var first = e.CurrentSelection.FirstOrDefault();
        if (first is not EventLink selectedEventLink)
        {
            return;
        }
        if (_schedulerBusinessLogic != null)
        {
            _schedulerBusinessLogic.SelectedEventLink = selectedEventLink;
        }
    }
}