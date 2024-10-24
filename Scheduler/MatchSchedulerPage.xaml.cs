using System.Collections.ObjectModel;
using CS341Project.Models;

namespace CS341Project.Scheduler;

/// <summary>
/// A page that displays a potential match schedule
/// for all matches in a given tournament.
/// </summary>
public partial class MatchSchedulerPage : ContentPage
{

    public ObservableCollection<Match> Matches { get; } = [];

    public MatchSchedulerPage()
    {
        InitializeComponent();
        MatchList.BindingContext = this;
        for (var i = 0; i < 10; i++)
        {
            Matches.Add(new Match(500, 1, 2, 600));
            Matches.Add(new Match(501, 3, 3, 601));
            Matches.Add(new Match(502, 5, 4, 602));
            Matches.Add(new Match(503, 6, 7, 700));
        }

    }
}