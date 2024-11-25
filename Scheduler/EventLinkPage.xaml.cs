using TOTools.Models;

namespace TOTools.Scheduler;

/// <summary>
/// An interface used to report the event link entered by the user.
/// </summary>
public interface IOnEventLinkSubmitted
{
    /// <summary>
    /// Called when the user adds an event link to the EventLinkPage.
    /// </summary>
    /// <param name="eventLink">The link that the user entered.</param>
    void OnEventLinkSubmitted(EventLink eventLink);
}

/// <summary>
/// A page that allows the user to add an event link and its required information for the scheduler.
/// </summary>
public partial class EventLinkPage : ContentPage
{
    private readonly IOnEventLinkSubmitted _onEventLinkSubmitted;

    public EventLinkPage(IOnEventLinkSubmitted onEventLinkSubmitted)
    {
        InitializeComponent();
        _onEventLinkSubmitted = onEventLinkSubmitted;

        // The default time for the event will be the current time
        var currentTime = DateTime.Now - DateTime.Today;
        StartTimeEntry.Time = currentTime;
    }

    /**
     * Created for editing an event, but not used TODO
     */
    public EventLinkPage(
        SchedulerEventPage onEventLinkSubmitted,
        EventLink selectedEvent
    ) : this(onEventLinkSubmitted)
    {
        EventLinkEntry.Text = selectedEvent.Link;
        var currentTime = DateTime.Now - DateTime.Today;
        var time = selectedEvent.StartTime.TimeOfDay;
        StartTimeEntry.Time = currentTime > time ? currentTime : time;
        ConcurrentMatchesEntry.Text = selectedEvent.NumberOfConcurrentMatches.ToString();
    }

    private void OnSubmitButtonClicked(object? sender, EventArgs e)
    {
        var eventLink = TryToParseEventLink();
        if (eventLink == null)
        {
            return;
        }

        _onEventLinkSubmitted.OnEventLinkSubmitted(eventLink);
        Navigation.PopAsync();
    }

    /// <summary>
    /// Tries to parse the event link submitted by the user.
    /// Any errors will be sent, via an alert, to the user.
    /// </summary>
    /// <returns>The event link used by the startgg API, otherwise null if there was invalid input</returns>
    private EventLink? TryToParseEventLink()
    {
        // Check link
        var linkText = EventLinkEntry.Text;
        if (string.IsNullOrWhiteSpace(linkText))
        {
            DisplayAlert("Invalid EventLink", "Please enter a valid event link", "OK");
            return null;
        }

        // Check start time
        var currentTime = DateTime.Now - DateTime.Today;
        var startTime = StartTimeEntry.Time;
#pragma warning disable CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
        if (startTime == null)
        {
            DisplayAlert("Invalid start time", "Please enter a valid start time", "OK");
            return null;
        }
#pragma warning restore CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
        if (startTime < currentTime)
        {
            DisplayAlert("Invalid start time", "Please enter a start time from the future", "OK");
            return null;
        }

        // Check concurrent matches
        var numberOfConcurrentMatchesText = ConcurrentMatchesEntry.Text;
#pragma warning disable CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
        if (numberOfConcurrentMatchesText == null)
        {
            DisplayAlert("Error", "Please enter the concurrent matches", "OK");
            return null;
        }
        
        //TODO
        // CHECK if Bo5 start placement is a valid DE placement, (ie, 1, 2, 3, 4, 5, 7, 9, 13, 17, 25, 33, 49, 65, etc)
        // Send to wherever it actually needs to go 
        
#pragma warning restore CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
        var parseSucceeded = int.TryParse(
            numberOfConcurrentMatchesText,
            out var numberOfConcurrentMatches);
        if (parseSucceeded && numberOfConcurrentMatches >= 1)
        {
            return new EventLink(
                EventLink.ExtractTournamentPath(linkText),
                DateTime.Today.Add(startTime),
                numberOfConcurrentMatches);
        }

        DisplayAlert("Error", "Please enter a valid number of concurrent matches", "OK");
        return null;
    }

    private void OnCancelButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}