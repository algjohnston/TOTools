using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Matches.Add(new Match("A", "E", 100));
            Matches.Add(new Match("B", "F", 101));
            Matches.Add(new Match("C", "G", 102));
            Matches.Add(new Match("D", "H", 103));
        }

    }
}