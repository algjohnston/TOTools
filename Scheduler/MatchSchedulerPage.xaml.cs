using System.Collections.ObjectModel;
using TOTools.Models;

namespace TOTools.Scheduler;

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