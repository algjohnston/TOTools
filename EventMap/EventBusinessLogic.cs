using System.Collections.ObjectModel;
using CS341Project.Database;
using CS341Project.Models;

namespace CS341Project.EventMap;

/// <summary>
/// Alexander Johnston
/// The business logic for the scheduler.
/// </summary>
public class EventBusinessLogic
{
    private readonly ITable<Event, long> _table = new EventTable();
    public ObservableCollection<Event> Events => _table.SelectAll();
    
}