using System.Collections.ObjectModel;
using CS341Project.Models;

namespace CS341Project.Scheduler;

/// <summary>
/// A page that displays a potential match schedule
/// for all matches in a given tournament.
/// </summary>
public partial class MatchSchedulerPage : ContentPage
{

    private SchedulerBusinessLogic _schedulerBusinessLogic = new();

    public MatchSchedulerPage()
    {
        InitializeComponent();
        MatchList.BindingContext = _schedulerBusinessLogic;
    }
}