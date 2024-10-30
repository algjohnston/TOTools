using System.Collections.ObjectModel;
using TOTools.Database;
using TOTools.Models;

namespace TOTools.EventMap;

/// <summary>
/// Alexander Johnston
/// The business logic for the scheduler.
/// </summary>
public class EventBusinessLogic
{
    private readonly ITable<Event, long> _table = new EventTable();
    public ObservableCollection<Event> Events => _table.SelectAll();
    
}