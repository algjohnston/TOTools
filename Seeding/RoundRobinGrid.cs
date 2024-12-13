using TOTools.Models.Startgg;

namespace TOTools.Seeding;

/// <summary>
/// A grid for a round-robin bracket.
/// </summary>
public class RoundRobinGrid : Grid
{
    
    /// <summary>
    /// Creates a round-robin grid.
    /// </summary>
    /// <param name="sets">The matches that will be or have been played in the round-robin.</param>
    /// <param name="color">The color of the grid row and column lines.</param>
    public RoundRobinGrid(List<Set> sets, Color color)
    {
        FillGridWithPlayers(sets, color);
    }

    /// <summary>
    /// Puts the player names and any winners in the grid.
    /// </summary>
    /// <param name="sets">The matches that will be or have been played in the round-robin.</param>
    /// <param name="color">The color of the grid row and column lines.</param>
    private void FillGridWithPlayers(List<Set> sets, Color color)
    {
        // Gets a list of unique players
        var playerNames = sets.Select(set => set.Player1DisplayName).ToList();
        playerNames.AddRange(sets.Select(set => set.Player2DisplayName));
        var uniquePlayers = playerNames.Distinct().ToList();

        // Set up the row and columns
        var numRowsAndColumns = uniquePlayers.Count + 1;
        RowDefinitions.Clear();
        ColumnDefinitions.Clear();
        for (var row = 0; row < numRowsAndColumns; row++)
        {
            RowDefinitions.Add(
                new RowDefinition { Height = GridLength.Star });
            ColumnDefinitions.Add(
                new ColumnDefinition { Width = GridLength.Star });
        }

        // Add the player names to the first row and column
        var playerIndex = 1;
        foreach (var player in uniquePlayers)
        {
            var player1Label = new Label
            {
                Text = player,
                TextColor = color,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            var player2Label = new Label
            {
                Text = player,
                TextColor = color,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            this.Add(player1Label, 0, playerIndex);
            this.Add(player2Label, playerIndex, 0);
            playerIndex++;
        }

        // Add the grid of winners
        var graphicsView = new GraphicsView
        {
            Drawable = new RoundRobinDrawable(uniquePlayers, sets, color),
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        this.AddWithSpan(
            graphicsView,
            1,
            1,
            uniquePlayers.Count,
            uniquePlayers.Count
        );
    }
}