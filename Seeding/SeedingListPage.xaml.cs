using System.Collections.ObjectModel;
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
        Shell.SetTabBarIsVisible(Shell.Current, true);
        SeedingList.Add(new PlayerTierGroup(
            "Tier S",
            [
                new Player(1, "Candle", "Madison", Tier.S, 1),
                new Player(3, "Comet", "MKE", Tier.S, 2),
                new Player(3, "Skuniar", "Norcen", Tier.S, 3)
            ]));
        SeedingList.Add(new PlayerTierGroup(
            "Tier A",
            [
                new Player(1, "CRB", "Norcen", Tier.A, 1),
                new Player(2, "Arico", "Norcen", Tier.A, 2),
                new Player(3, "Spencer", "MKE", Tier.A, 3)
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