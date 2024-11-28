using System.Collections.ObjectModel;
using System.Windows.Input;
using GraphQL.Client.Http;
using TOTools.Models;

namespace TOTools.Scheduler;

/// <summary>
/// A page that displays a potential match schedule for all matches in a list of events
/// that were input by the user in the SchedulerEventPage.
/// </summary>
public partial class MatchSchedulerPage : ContentPage, INewTimeSubmitted
{
    private SchedulerBusinessLogic? _schedulerBusinessLogic;

    public MatchSchedulerPage()
    {
        InitializeComponent();
        HandlerChanged += OnHandlerChanged;
    }

    private async void OnHandlerChanged(object? sender, EventArgs e)
    {
        _schedulerBusinessLogic ??= Handler?.MauiContext?.Services
            .GetService<SchedulerBusinessLogic>();
        if (_schedulerBusinessLogic == null)
        {
            return;
        }

        // Need to make sure the past matches are loaded for the scheduler to estimate time
        await _schedulerBusinessLogic.PastMatchLoadTask;
        // Since the user already entered the event links, and they are in the SchedulerBusinessLogic,
        // we can load the schedule right away
        await _schedulerBusinessLogic.LoadPotentialSchedule();
        MatchList.BindingContext = _schedulerBusinessLogic;
    }

    private void OnReportButtonClicked(object? sender, EventArgs e)
    {
        if (_schedulerBusinessLogic.SelectedMatch != null)
        {
            new ReportMatchPopup(_schedulerBusinessLogic.SelectedMatch);
        }
    }

    private void OnStartButtonClicked(object? sender, EventArgs e)
    {
        if (_schedulerBusinessLogic == null) return;
        if (_schedulerBusinessLogic?.SelectedMatch == null) return;
        _schedulerBusinessLogic.SelectedMatch.MatchStartTime = DateTime.Now;
        _schedulerBusinessLogic.SelectedMatch.IsInProgress = true;

    }

    private void OnMatchSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Match selectedMatch)
        {
            _schedulerBusinessLogic.SelectedMatch = selectedMatch;
        }
    }

    /// <summary>
    /// Expands or collapses a group.
    /// </summary>
    /// <param name="sender">
    /// Should be the Label with a binding context set to
    /// the player group to be expanded or collapsed.
    /// </param>
    /// <param name="e">Ignored</param>
    private void OnToggleGroupClick(object sender, EventArgs e)
    {
        if (sender is not Label { BindingContext: EventMatchGroup eventMatchGroup }) return;
        eventMatchGroup.ToggleExpanded();

        // Needed to get the list to refresh
        // since the ObservableCollection does not listen for changes in elements
        // (even PropertyChanged events sent by the elements do not trigger a change
        //  and there is no way to manually link the events or trigger a refresh) 
        MatchList.ItemsSource = null;
        MatchList.ItemsSource = _schedulerBusinessLogic.FutureMatches;
    }
    
    private void OnDragMatchGroupStarting(object? sender, DragStartingEventArgs e)
    {
        if (sender is not DragGestureRecognizer { BindingContext: EventMatchGroup eventMatchGroup }) return;
        e.Data.Properties["EventGroup"] = eventMatchGroup;
    }


    private void OnDropMatchGroup(object? sender, DropEventArgs e)
    {
        if (sender is not DropGestureRecognizer { BindingContext: EventMatchGroup destinationPlayerGroup }) return;
        var sourceGroup = (EventMatchGroup)e.Data.Properties["EventGroup"];
        
        var sourceIndex = _schedulerBusinessLogic.FutureMatches.IndexOf(sourceGroup);
        var destinationIndex = _schedulerBusinessLogic.FutureMatches.IndexOf(destinationPlayerGroup);
        if (sourceIndex == destinationIndex)
        {
            return;
        }
        
        _schedulerBusinessLogic.FutureMatches.Move(sourceIndex, destinationIndex);
        // Needed to update the list this way because the CollectionView can not handle the above code
        MatchList.ItemsSource = null;
        MatchList.ItemsSource = _schedulerBusinessLogic.FutureMatches;
    }

    private void OnChangeStartTimeClicked(object? sender, EventArgs e)
    {
        var selectedMatch = _schedulerBusinessLogic.SelectedMatch;
        EventMatchGroup? eventMatchGroup = null;
        foreach (var group in _schedulerBusinessLogic.FutureMatches)
        {
            if (group.Contains(selectedMatch))
            {
                eventMatchGroup = group;
                break;
            }
        }
        Navigation.PushAsync(new ChangeEventTimePage(eventMatchGroup, this));
    }

    public void NewTimeSubmitted()
    {
        MatchList.ItemsSource = null;
        MatchList.ItemsSource = _schedulerBusinessLogic.FutureMatches;
    }
}