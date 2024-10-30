using System.Collections.ObjectModel;
using TOTools.Database;
using TOTools.Models;

namespace TOTools.Scheduler;

/// <summary>
/// Business logic for the scheduler
/// </summary>
public class SchedulerBusinessLogic
{
    private readonly ITable<Match, long> _table = new MatchTable();
        
    // this has all matches played previously by any players in any game, used to estimate time
    public ObservableCollection<Match> pastMatches => _table.SelectAll();
    // This gets filled with matches from startgg that need to be played
    public ObservableCollection<Match> currentMatches { get; }  = [];
}