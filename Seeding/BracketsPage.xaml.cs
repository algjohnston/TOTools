using System.Collections.ObjectModel;
using TOTools.Models;

namespace TOTools.Seeding;

/// <summary>
/// A page that displays the winner's and loser's bracket of a double elimination tournament.
/// </summary>
public partial class BracketsPage : ContentPage
{
    public ObservableCollection<BracketGroup> BracketGroupList { get; } = [];

    private SeedingBusinessLogic? _seedingBusinessLogic;

    public BracketsPage()
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

        // Waits for players to load
        await _seedingBusinessLogic.PlayerLoadTask;

        // Goes through every event's brackets
        // TODO may want to group by event
        List<string> roundRobinBracketIdentifiers = [];
        List<string> doubleEliminationBracketIdentifiers = [];
        foreach (var eventBracketGroup in _seedingBusinessLogic.EventBrackets)
        {
            // Adds all the round-robin bracket identifiers as a group
            var roundRobinBrackets = eventBracketGroup.GetRoundRobinBrackets();
            roundRobinBracketIdentifiers.AddRange(
                roundRobinBrackets.Select(
                    bracket => bracket.First().DisplayIdentifier
                )
            );

            // Adds all the double elimination bracket identifiers as a group
            var doubleEliminationBrackets = eventBracketGroup.GetDoubleEliminationLoserWinner();
            doubleEliminationBracketIdentifiers.AddRange(
                doubleEliminationBrackets.Select(
                    winnerSet => winnerSet.DisplayIdentifier
                )
            );
        }

        BracketGroupList.Clear();
        BracketGroupList.Add(new BracketGroup("Round Robin", roundRobinBracketIdentifiers));
        BracketGroupList.Add(new BracketGroup("Double Elimination", doubleEliminationBracketIdentifiers));
    }

    private void OnToggleGroupClick(object sender, EventArgs e)
    {
        if (sender is not Label { BindingContext: BracketGroup bracketGroup }) return;
        bracketGroup.ToggleExpanded();

        // Needed to get the list to refresh
        // since the ObservableCollection does not listen for changes in elements
        // (even PropertyChanged events sent by the elements do not trigger a change
        //  and there is no way to manually link the events or trigger a refresh) 
        BracketListView.ItemsSource = null;
        BracketListView.ItemsSource = BracketGroupList;
    }

    private void OnLabelClick(object? sender, TappedEventArgs e)
    {
        if (_seedingBusinessLogic == null || sender is not Label { BindingContext: string displayIdentifier }) return;
        _seedingBusinessLogic.SetActiveBracketForEditing(displayIdentifier);
        Navigation.PushAsync(new BracketEditorPage());
    }
}