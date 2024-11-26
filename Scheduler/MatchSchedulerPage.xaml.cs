﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using GraphQL.Client.Http;
using TOTools.Models;

namespace TOTools.Scheduler;

/// <summary>
/// A page that displays a potential match schedule for all matches in a list of events
/// that were input by the user in the SchedulerEventPage.
/// </summary>
public partial class MatchSchedulerPage : ContentPage
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
    
    public void OnReportButtonClicked(object? sender, EventArgs e)
    {
        // TODO 
        // make a popup that lets you report who won the set
    }

    public void OnStartButtonClicked(object? sender, EventArgs e)
    {
        _schedulerBusinessLogic.SelectedMatch.MatchStartTime = DateTime.Now;
        _schedulerBusinessLogic.SelectedMatch.IsInProgress = true;

    }
    
    public void OnMatchSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Match selectedMatch)
        {
            _schedulerBusinessLogic.SelectedMatch = selectedMatch;
        }
    }
    
}