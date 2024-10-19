namespace CS341Project.Models;

/// <summary>
/// Alexander Johnston
/// Represents a tournament event.
/// </summary>
/// <param name="eventId">The unique id of the event.</param>
/// <param name="eventName">The name of the event.</param>
/// <param name="location">The location of the event.</param>
/// <param name="startDateAndTime"></param>
/// <param name="endDateAndTime"></param>
public class Event(
    long eventId,
    string eventName,
    string location,
    DateTime startDateAndTime,
    DateTime endDateAndTime)
{
    public long EventId { get; } = eventId;
    public string EventName { get; } = eventName;
    public string Location { get; } = location;
    public DateTime StartDateAndTime { get; } = startDateAndTime;
    public DateTime EndDateAndTime { get; } = endDateAndTime;
}