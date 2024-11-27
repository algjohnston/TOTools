using CommunityToolkit.Maui.Views;
using TOTools.Models;

namespace TOTools.Seeding;

/// <summary>
/// A callback for when a new player is added via the PlayerEditorPopup.
/// </summary>
public interface IOnPlayerAdded
{
    void OnPlayerUpdated(Player player);
}

/// <summary>
/// A popup for when a player needs to be added to the player table,
/// or when a player in the player table needs to be updated.
/// </summary>
public partial class PlayerEditorPopup : Popup
{
    private readonly IOnPlayerAdded _onPlayerAdded;

    private Player _player;
    
    /// <summary>
    /// Shows a popup for editing the given player.
    /// </summary>
    /// <param name="onPlayerAdded">Called when the player has been submitted by the user.</param>
    /// <param name="player">The player to edit.</param>
    /// <param name="height">The desired height of the popup.</param>
    /// <param name="width">The desired width of the popup.</param>
    public PlayerEditorPopup(
        IOnPlayerAdded onPlayerAdded,
        Player player,
        double height,
        double width
    )
    {
        InitializeComponent();
        _player = player;
        BindingContext = player;
        _onPlayerAdded = onPlayerAdded;
        PopupGrid.HeightRequest = height;
        PopupGrid.WidthRequest = width;
    }

    private void OnOkButtonClicked(object? sender, EventArgs e)
    {
        // TODO validation
        
        _onPlayerAdded.OnPlayerUpdated(
            new Player(
                _player.StarttggId,
                TagEntry.Text,
                RegionHelper.ConvertToRegion(RegionPicker.SelectedIndex),
                TierHelper.ConvertToTier(TierPicker.SelectedIndex),
                -1
            )
        );
        Close();
    }

    private void OnCancelButtonClicked(object? sender, EventArgs e)
    {
        Close();
    }
}