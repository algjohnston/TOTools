using System.Collections.ObjectModel;
using Mapsui.Tiling;
using TOTools.Database;
using TOTools.Models;
namespace TOTools.EventMap;

/// <summary>
/// Alexander Johnston
/// The business logic for the scheduler.
/// </summary>
public class EventBusinessLogic(EventTable eventTable)
{
    public ObservableCollection<Event> Events { get; } = [];
    private readonly TaskCompletionSource<bool> _loadCompletionSource = new();
    public Task LoadTask => _loadCompletionSource.Task;

    /// <summary>
    /// Loads all events from the database.
    /// This method is called at app start to avoid lag.
    /// To know when loading is done, the LoadTask must be awaited.
    /// </summary>
    public void LoadEvents()
    {
        var events = eventTable.SelectAll();
        Events.Clear();
        foreach (var singleEvent in events)
        {
            Events.Add(singleEvent);
        }

        _loadCompletionSource.TrySetResult(true);
    }

    public async void AddEvent(string eventName, string eventAddress, string eventLink)
    {
        IEnumerable<Location?> locations = await Geocoding.Default.GetLocationsAsync(eventAddress);
        var venueLocation = locations?.FirstOrDefault();
        if (venueLocation == null)
        {
            return;
        }
        // We probably don't need the Date for the event, since events being added to this will be weekly, or monthly or whatever, and the 
        // date and how frequently they occur will all be on the startgg page, which they will give the link for anyway
        // we will leave it the db, and thus here as well, just in case the clients decide to change their minds later (may happen after final submission)
        long id = (Events.Count == 0) ? 0 : Events.Last().EventId + 1;
        var eventToAdd = new Event(id, eventName, eventAddress, DateTime.Now, DateTime.Now, venueLocation.Latitude,
            venueLocation.Longitude, eventLink);
        eventTable.Insert(eventToAdd);
        Events.Add(eventToAdd);
        AddPin(eventToAdd);
    }


    public void RemoveEvent(Event eventToRemove)
    {
        eventTable.Delete(eventToRemove.EventId);
        RemovePin(eventToRemove);
    }

    private void AddPin(Event eventToAdd)
    {
        
    }

    private void RemovePin(Event eventToRemove)
    {
        
    }
    
}