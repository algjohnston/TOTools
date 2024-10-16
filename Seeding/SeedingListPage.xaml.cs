﻿using System.Collections.ObjectModel;
using CS341Project.Models;

namespace CS341Project.Seeding;

/// <summary>
/// A page of people who have competed in tournaments in the past.
/// The people are put in groups based on tier that can be expanded or collapsed.
/// </summary>
public partial class SeedingListPage : ContentPage
{
    public ObservableCollection<PlayerTierGroup> SeedingList { get; } = [];

    public SeedingListPage()
    {
        InitializeComponent();
        BindingContext = this;
        SeedingList.Add(new PlayerTierGroup(
            "Tier 1",
            [
                new Player("A", "R0"),
                new Player("B", "R1"),
                new Player("C", "R2")
            ]));
        SeedingList.Add(new PlayerTierGroup(
            "Tier 2",
            [
                new Player("D", "R3"),
                new Player("E", "R2"),
                new Player("F", "R1")
            ]));
    }

    /// <summary>
    /// Expands or collapses a group.
    /// </summary>
    /// <param name="sender">
    /// Should be the Label with a binding context set to
    /// the player group to be expanded or collapsed
    /// </param>
    /// <param name="e">Ignored</param>
    private void ToggleGroup(object sender, EventArgs e)
    {
        if (sender is not Label { BindingContext: PlayerTierGroup playerGroup }) return;
        playerGroup.ToggleExpanded();
        // Needed to get the list to refresh
        // since the ObservableCollection does not listen for changes in elements
        // (even PropertyChanged events sent by the elements do not trigger a change
        //  and there is no way to manually link the events or trigger a refesh) 
        SeedingListView.ItemsSource = null;
        SeedingListView.ItemsSource = SeedingList;
    }
}