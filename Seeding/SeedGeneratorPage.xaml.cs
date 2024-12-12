using CommunityToolkit.Maui.Views;
using TOTools.Models;

namespace TOTools.Seeding;

/// <summary>
/// A page that takes in a list of players and produces a double elimination bracket.
/// </summary>
public partial class SeedGeneratorPage : ContentPage, IOnPlayerAdded
{
    /**
     * TODO There should be a toggle to allow for displaying all brackets from an event vs creating one instead
     * Currently just displays all brackets in a link.
     */
    private SeedingBusinessLogic? _seedingBusinessLogic;

    public SeedGeneratorPage()
    {
        InitializeComponent();
        AttendeeLinkEntry.Text = // For testing
            "tournament/la-mechita-2024-torneo-feria-esports-zona-retro-y-m/event/ultimate-singles";
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

        // Wait for the players to get loaded
        await _seedingBusinessLogic.PlayerLoadTask;
        BindingContext = _seedingBusinessLogic;
    }

    private readonly SemaphoreSlim _semaphore = new(1, 1);

    private async void OnSubmitButtonClicked(object? sender, EventArgs e)
    {
        if (!await _semaphore.WaitAsync(0))
        {
            return;
        }

        try
        {
            var linkText = AttendeeLinkEntry.Text;
            if (string.IsNullOrEmpty(linkText))
            {
                await DisplayAlert("Invalid BracketLink", "Please enter a valid event link", "OK");
                return;
            }

            await _seedingBusinessLogic!.AddLinkPhaseGroups(linkText);
            var players = _seedingBusinessLogic.GetUnknownPlayers();
            var screenHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
            var screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            foreach (var player in players)
            {
                await this.ShowPopupAsync(
                    new PlayerEditorPopup(
                        this,
                        player,
                        screenHeight * 0.75,
                        screenWidth * 0.75
                    )
                );
            }

            await Navigation.PushAsync(new BracketsPage());
        }
        finally
        {
            _semaphore.Release();
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _seedingBusinessLogic?.ClearBrackets();
    }

    public void OnPlayerUpdated(Player player)
    {
        _seedingBusinessLogic?.AddOrUpdatePlayer(player);
    }
}