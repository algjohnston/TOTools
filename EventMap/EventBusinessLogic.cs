using System.Collections.ObjectModel;
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
}