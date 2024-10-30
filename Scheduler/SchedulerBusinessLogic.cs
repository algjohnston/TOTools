using System.Collections.ObjectModel;
using CS341Project.Database;
using CS341Project.Models;

namespace CS341Project.Scheduler;

/// <summary>
/// Business logic for the scheduler
/// </summary>
public class SchedulerBusinessLogic
{
    private readonly ITable<Match, long> _table = new MatchTable();
    public ObservableCollection<Match> Matches => _table.SelectAll();
}