namespace TOTools.Models;

/// <summary>
/// Alexander Johnston
/// Represents a tournament event link input by the user.
/// </summary>
/// <param name="link">A link to startgg that has the event data.</param>
/// <param name="startTime">The time the event start.</param>
/// <param name="numberOfConcurrentMatches">The number of matches that can be played at the same time.</param>
/// 
public class EventLink(string link, DateTime startTime, int numberOfConcurrentMatches)
{
    public string Link { get; } = link;
    public DateTime StartTime { get; } = startTime;

    public string StartTimeFormatted { get; } = startTime.ToShortTimeString();
    
    public int NumberOfConcurrentMatches { get; } = numberOfConcurrentMatches;
}