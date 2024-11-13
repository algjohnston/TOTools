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
        var startTime = StartTimeEntry.Time;
        if (startTime == null)
        {
            DisplayAlert("Invalid start time", "Please enter a valid start time", "OK");
            return null;
        }

        if (startTime < currentTime)
        {
            DisplayAlert("Invalid start time", "Please enter a start time from the future", "OK");
            return null;
        }

        return new EventLink(
            ExtractTournamentPath(link),
            DateTime.Today.Add(startTime));
    }

    private static string ExtractTournamentPath(string link)
    {
        const string startKeyword = "tournament/";
        const string eventEndKeyword = "/event/";

        var result = link;
        var startIndex = link.IndexOf(startKeyword, StringComparison.Ordinal);
        if (startIndex != -1)
        {
            result = link[startIndex..];
        }

        // Remove anything after "/event/[some string]/" including the last slash
        var eventIndex = result.IndexOf(eventEndKeyword, StringComparison.Ordinal);
        if (eventIndex == -1) return result;
        var endIndex = result.IndexOf(
            '/',
            eventIndex + eventEndKeyword.Length);
        if (endIndex != -1)
        {
            result = result[..endIndex];
        }
        return result;
    }

    private void OnCancelButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}