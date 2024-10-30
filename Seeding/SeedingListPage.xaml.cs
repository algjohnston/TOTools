using System.Collections.ObjectModel;
using CS341Project.Models;
using Region = CS341Project.Models.Region;

namespace CS341Project.Seeding;

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
        SeedingList.Add(new PlayerTierGroup(
            "Tier S",
            [
                new Player("a","Candle", Region.West, Tier.S, 1),
                new Player("b","Comet", Region.West, Tier.S, 2),
                new Player("c","Skuniar", Region.West, Tier.S, 3)
            ]));
        SeedingList.Add(new PlayerTierGroup(
            "Tier A",
            [
                new Player("a","CRB", Region.West, Tier.A, 1),
                new Player("a","Arico", Region.West, Tier.A, 2),
                new Player("a","Spencer", Region.West, Tier.A, 3)
            ]));
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