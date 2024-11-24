using TOTools.Models;

namespace TOTools.Scheduler;

public interface IOnEventLinkSubmitted
{
    void OnEventLinkSubmitted(EventLink eventLink);
}

public partial class EventLinkPopup : ContentPage
{
    private readonly IOnEventLinkSubmitted _onEventLinkSubmitted;

    public EventLinkPopup(IOnEventLinkSubmitted onEventLinkSubmitted)
    {
        InitializeComponent();
        _onEventLinkSubmitted = onEventLinkSubmitted;
        var currentTime = DateTime.Now - DateTime.Today;
        StartTimeEntry.Time = currentTime;
    }

    public EventLinkPopup(
        SchedulerEventPage onEventLinkSubmitted, 
        EventLink selectedEvent
        ) : this(onEventLinkSubmitted)
    {
        EventLinkEntry.Text = selectedEvent.Link;
        var currentTime = DateTime.Now - DateTime.Today;
        var time = selectedEvent.StartTime.TimeOfDay;
        if (currentTime > time)
        {
            StartTimeEntry.Time = currentTime;
        }
        else
        {
            StartTimeEntry.Time = time;
        }
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

    private EventLink? TryToParseEventLink()
    {
        var link = EventLinkEntry.Text;
        if (string.IsNullOrWhiteSpace(link))
        {
            DisplayAlert("Invalid EventLink", "Please enter a valid event link", "OK");
            return null;
        }

        var currentTime = DateTime.Now - DateTime.Today;
        TimeSpan startTime = StartTimeEntry.Time;
        
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

        var numberOfConcurrentMatchesText = ConcurrentMatchesEntry.Text;
        
#pragma warning disable CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
        if (numberOfConcurrentMatchesText == null)
        {
            DisplayAlert("Error", "Please enter the concurrent matches", "OK");
            return null;
        }
#pragma warning restore CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
        
        var parseSucceeded = int.TryParse(numberOfConcurrentMatchesText, out var numberOfConcurrentMatches);
        if (parseSucceeded && numberOfConcurrentMatches >= 1)
        {
            return new EventLink(
                EventLink.ExtractTournamentPath(link),
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