﻿using System.Collections.ObjectModel;
using TOTools.Models;

namespace TOTools.Seeding;

/// <summary>
/// A page of players who have competed in past tournaments.
/// The players are put in groups based on the tier they belong to
/// and each group can be expanded or collapsed.
/// </summary>
public partial class SeedingListPage : ContentPage
{
    private SeedingBusinessLogic? _seedingBusinessLogic;
    
    public ObservableCollection<PlayerTierGroup> PlayerTierGroups { get; } = [];

    public SeedingListPage()
    {
        InitializeComponent();
        BindingContext = this;
        HandlerChanged += OnHandlerChanged;
    }

    private async void OnHandlerChanged(object? sender, EventArgs e)
    {
        _seedingBusinessLogic ??= Handler?.MauiContext?.Services
            .GetService<SeedingBusinessLogic>();
        if (_seedingBusinessLogic == null)
        {
            return;
        }
        // Need to make sure the players have been loaded
        await _seedingBusinessLogic.PlayerLoadTask;
        PopulateTiers();
    }

    /// <summary>
    /// Groups players by their tier group and makes it available to the UI.
    /// </summary>
    private void PopulateTiers()
    {
        // Group player by tier
        var players = _seedingBusinessLogic!.Players;
        var groupedPlayers = players
            .GroupBy(p => p.PlayerTier)
            .ToDictionary(p => p.Key, p => p.ToList());
        
        // Populate each tier group
        foreach (Tier tier in Enum.GetValues(typeof(Tier)))
        {
            var tierString = "Tier " + TierConverter.ToString(tier);
            // Handle the case where there are no players in a tier
            if (!groupedPlayers.TryGetValue(tier, out var playerGroup))
            {
                PlayerTierGroups.Add(new PlayerTierGroup(tierString, []));
                continue;
            }
            // Add the player groups to their respective tier group
            var currentTierGroup = new PlayerTierGroup(tierString, playerGroup);
            currentTierGroup.Sort();
            PlayerTierGroups.Add(currentTierGroup);
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
        if (sender is not Label { BindingContext: PlayerTierGroup playerGroup }) return;
        playerGroup.ToggleExpanded();

        // Needed to get the list to refresh
        // since the ObservableCollection does not listen for changes in elements
        // (even PropertyChanged events sent by the elements do not trigger a change
        //  and there is no way to manually link the events or trigger a refresh) 
        SeedingListView.ItemsSource = null;
        SeedingListView.ItemsSource = PlayerTierGroups;
    }
    
    private void OnDragTierGroupStarting(object? sender, DragStartingEventArgs e)
    {
        if (sender is not DragGestureRecognizer { BindingContext: PlayerTierGroup playerGroup }) return;
        e.Data.Properties["TierGroup"] = playerGroup;
    }


    private void OnDropTierGroup(object? sender, DropEventArgs e)
    {
        if (sender is not DropGestureRecognizer { BindingContext: PlayerTierGroup destinationPlayerGroup }) return;
        var sourceGroup = (PlayerTierGroup)e.Data.Properties["TierGroup"];
        
        var sourceIndex = PlayerTierGroups.IndexOf(sourceGroup);
        var destinationIndex = PlayerTierGroups.IndexOf(destinationPlayerGroup);
        if (sourceIndex == destinationIndex)
        {
            return;
        }
        
        PlayerTierGroups.Remove(sourceGroup);
        PlayerTierGroups.Remove(destinationPlayerGroup);
        PlayerTierGroups.Insert(sourceIndex, destinationPlayerGroup);
        PlayerTierGroups.Insert(destinationIndex, sourceGroup);
        
        // Needed to update the list this way because the CollectionView can not handle the above code
        SeedingListView.ItemsSource = null;
        SeedingListView.ItemsSource = PlayerTierGroups;
    }
}