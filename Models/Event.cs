namespace CS341Project.Models;

/// <summary>
/// Alexander Johnston
/// Represents a tournament event.
/// </summary>
/// <param name="link">A link to startgg that has the event data.</param>
/// <param name="startTime">The time the event start.</param>
public class Event(string link, DateTime startTime)
{
    public string Link { get; } = link;
    public string StartTime { get; } = startTime.ToShortTimeString();
}