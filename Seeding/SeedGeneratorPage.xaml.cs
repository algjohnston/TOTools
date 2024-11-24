namespace TOTools.Seeding;

/// <summary>
/// A page that takes in a list of players and produces a double elimination bracket.
/// </summary>
public partial class SeedGeneratorPage : ContentPage
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
            "tournament/between-2-lakes-67-a-madison-super-smash-bros-tournament/event/ultimate-singles";
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
        BindingContext = _seedingBusinessLogic;
    }

    private async void OnSubmitButtonClicked(object? sender, EventArgs e)
    {
        var linkText = AttendeeLinkEntry.Text;
        if (string.IsNullOrEmpty(linkText))
        {
            await DisplayAlert("Invalid BracketLink", "Please enter a valid event link", "OK");
            return;
        }

        await _seedingBusinessLogic!.AddLink(linkText);
        await Navigation.PushAsync(new BracketsPage());
    }

    private void OnManualEntryButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new SelectCompetitorsPage());
    }
    
}