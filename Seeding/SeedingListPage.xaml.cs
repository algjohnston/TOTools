using System.Collections.ObjectModel;
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
        await _seedingBusinessLogic.LoadTask;
        PopulateTiers();
    }

    private void PopulateTiers()
    {
        var tierConverter = new TierConverter();
        var players = _seedingBusinessLogic!.Players;
        
        var groupedPlayers = players
            .GroupBy(p => p.PlayerTier)
            .ToDictionary(p => p.Key, p => p.ToList());
        
        foreach (Tier tier in Enum.GetValues(typeof(Tier)))
        {
            var tierString = "Tier " + tierConverter.ToString(tier);
            if (!groupedPlayers.TryGetValue(tier, out var playerGroup))
            {
                PlayerTierGroups.Add(new PlayerTierGroup(tierString, []));
                continue;
            }
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
    
}