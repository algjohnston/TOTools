using System.Collections.ObjectModel;
using CS341Project.Models;

namespace CS341Project.EventMap;

/// <summary>
/// Alexander Johnston
/// The business logic for the scheduler
/// </summary>
public class EventBusinessLogic
{
    private readonly IDatasource<Event> datasource = new EventDatasource();
    public ObservableCollection<Event> Events => datasource.SelectAll();
    
}