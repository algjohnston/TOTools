using System.Collections.ObjectModel;
using CS341Project.Database;
using CS341Project.Models;

namespace CS341Project.EventMap;

/// <summary>
/// Alexander Johnston
/// The business logic for the scheduler
/// </summary>
public class EventBusinessLogic
{
    private readonly IDatabaseTable<Event> _databaseTable = new EventDatabaseTable();
    public ObservableCollection<Event> Events => _databaseTable.SelectAll();
    
}