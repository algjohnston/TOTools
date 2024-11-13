﻿using System.Collections.ObjectModel;
using TOTools.Models;

namespace TOTools.Scheduler;

/// <summary>
/// A page with a list of events that are used to come up with a match schedule.
/// </summary>
public partial class SchedulerEventPage : ContentPage, IOnEventLinkSubmitted
{
    public ObservableCollection<EventLink> Events { get; } = [];

    public SchedulerEventPage()
    {
        InitializeComponent();
        BindingContext = this;

        // For testing
        Events.Add(
            new EventLink(
                "https://www.start.gg/tournament/between-2-lakes-67-a-madison-super-smash-bros-tournament/event/ultimate-singles/overview",
                DateTime.Now)
        );
    }

    private void OnSettingsImageButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new EventPopup());
    }

    private void OnSubmitButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new MatchSchedulerPage());
    }

    private void OnAddLinkButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new EventLinkPopup(this));
    }

    public void OnEventLinkSubmitted(EventLink eventLink)
    {
        Events.Add(eventLink);
    }
}