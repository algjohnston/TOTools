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