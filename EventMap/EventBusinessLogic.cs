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
    public ObservableCollection<Event> Events => eventTable.SelectAll();
    
}