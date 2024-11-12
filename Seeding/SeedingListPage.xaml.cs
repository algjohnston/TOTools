using System.Collections.ObjectModel;
using TOTools.Models;
using Region = TOTools.Models.Region;

namespace TOTools.Seeding;

/// <summary>
/// A page of players who have competed in past tournaments.
/// The players are put in groups based on the tier they belong to
/// and each group can be expanded or collapsed.
/// </summary>
public partial class SeedingListPage : ContentPage
{ 
    public ObservableCollection<PlayerTierGroup> SeedingList { get; } = [];

    public SeedingListPage()
    {
        InitializeComponent();
        BindingContext = this;
        PopulateTiers();
    }

    public void PopulateTiers()
    {
        var playerTable = PlayerTable.GetPlayerTable();
        var tierConverter = new TierConverter();
        var players = playerTable.SelectAll();
        foreach (Tier tier in Enum.GetValues(typeof(Tier)))
        {
            var currentTierGroup = new PlayerTierGroup("Tier " + tierConverter.ToString(tier),
                players.Where(p => p.PlayerTier == tier));
            currentTierGroup.Sort();
            SeedingList.Add(currentTierGroup);
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
        SeedingListView.ItemsSource = SeedingList;
    }
}