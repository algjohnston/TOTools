namespace TOTools.Models;

/// <summary>
/// Alexander Johnston
/// Represents a tournament event.
/// </summary>
/// <param name="eventId">The unique id of the event.</param>
/// <param name="eventName">The name of the event.</param>
/// <param name="location">The location of the event.</param>
/// <param name="startDateTime">The start date time of the event.</param>
/// <param name="endDateTime">The end date time of the event.</param>
/// <param name="latitude">The latitude of the event.</param>
/// <param name="longitude">The longitude of the event.</param>
public class Event(
    long eventId,
    string eventName,
    string location,
    DateTime startDateTime,
    DateTime endDateTime,
    double latitude,
    double longitude)
{
    public long EventId { get; } = eventId;
    public string EventName { get; } = eventName;
    public string Location { get; } = location;
    public DateTime StartDateTime { get; } = startDateTime;
    public DateTime EndDateTime { get; } = endDateTime;
    public string FormattedStartDateTime => StartDateTime.ToString("g");
    public string FormattedEndDateTime => EndDateTime.ToString("g");
    public double Latitude { get; } = latitude;
    public double Longitude { get; } = longitude;
}