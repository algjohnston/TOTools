namespace TOTools.Models;

/// <summary>
/// Alexander Johnston
/// An expandable group of matches sharing the event.
/// </summary>
/// <param name="eventName">The name of the event the matches are in.</param>
/// <param name="startTime">The start time of the event.</param>
/// <param name="matches">The matches in the given event.</param>
public class EventMatchGroup(
    string eventName,
    DateTime startTime,
    IEnumerable<Match> matches
) : List<Match>(matches)
{
    public string EventName { get; } = eventName;

    public DateTime StartTime { get; set; } = startTime;
    
    public string FormattedEventWithTime => EventName + ": "  + StartTime.ToShortTimeString();
    
    private bool Expanded { get; set; } = true;

    public void ToggleExpanded()
    {
        if (!Expanded)
        {
            AddRange(matches);
        }
        else
        {
            Clear();
        }

        Expanded = !Expanded;
    }
}