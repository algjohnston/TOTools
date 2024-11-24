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
    
    /// <summary>
    /// Extracts the tournament path needed by startgg from an event link.
    /// </summary>
    /// <param name="link">The startgg event link.</param>
    /// <returns></returns>
    public static string ExtractTournamentPath(string link)
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
}