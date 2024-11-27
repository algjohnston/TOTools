using TOTools.Models.Startgg;

namespace TOTools.Seeding;

public class RoundRobinGrid : Grid
{
    public RoundRobinGrid(List<Set> sets, Color color)
    {
        FillGridWithPlayers(sets, color);
    }

    private void FillGridWithPlayers(List<Set> sets, Color color)
    {
        var playerNames = sets.Select(set => set.Player1).ToList();
        playerNames.AddRange(sets.Select(set => set.Player2));
        var uniquePlayers = playerNames.Distinct().ToList();

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

        var playerIndex = 1;
        foreach (var player in uniquePlayers)
        {
            var winnerLabel = new Label
            {
                Text = player,
                TextColor = color,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            var winnerLabel2 = new Label
            {
                Text = player,
                TextColor = color,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            this.Add(winnerLabel, 0, playerIndex);
            this.Add(winnerLabel2, playerIndex, 0);
            playerIndex++;
        }

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