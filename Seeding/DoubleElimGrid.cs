namespace TOTools.Seeding;

/// <summary>
/// A page that displays a double elimination bracket.
/// </summary>
public class DoubleElimGrid : Grid
{
    public DoubleElimGrid()
    {
        FillGridWithPlayers();
    }

    private void FillGridWithPlayers()
    {
        List<string> players =
        [
            "A", "B", "C", "D",
            "A", "B", "C", "D",
            "A", "B", "C", "D",
            "A", "B", "C", "D"
        ];
        var numberOfPlayers = players.Count;

        // Add the rows
        RowDefinitions.Clear();
        // To ensure the columns after the first have the names centered,
        // an extra row in between names is used.
        // |name--|
        // |      |--name
        // |name--|
        var numberOfRows = 2 * numberOfPlayers;
        for (var row = 0; row < numberOfRows; row++)
        {
            RowDefinitions.Add(
                new RowDefinition { Height = GridLength.Star });
        }

        ColumnDefinitions.Clear(); // Add the columns
        // To ensure the columns in between the names have the lines connecting players,
        // an extra column in between is used.
        //          v--------- column with lines 
        // |  0  |  1  |  2  |
        // | name --|
        // |        |-- name |
        // | name --|
        // The extra column is for the winner's name
        var numberOfColumns = (2 * (int)Math.Log(numberOfPlayers, 2)) + 1;
        for (var i = 0; i < numberOfColumns; i++)
        {
            ColumnDefinitions.Add(
                new ColumnDefinition { Width = GridLength.Star });
        }


        for (
            var currentPlayerNumber = 0;
            currentPlayerNumber < numberOfPlayers;
            currentPlayerNumber++) // Fill the first column with players
        {
            // TODO autoscale text?
            var playerLabel = new Label
            {
                Text = players[currentPlayerNumber],
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            
            this.Add(
                playerLabel,
                0,
                (currentPlayerNumber * 2)
                );
        }

        // Fill the winner columns and add the lines in between names
        var currentRowCount = numberOfPlayers / 2;
        var winnerColumnsCount = numberOfColumns / 2;
        for (var col = 0; col < winnerColumnsCount; col++)
        {
            var currentColumn = (2 * col) + 1;

            var rowOffsetForLines = (int)Math.Pow(2, col) - 1;
            var rowSpanForLines = (int)Math.Pow(2, col + 1) + 1;

            var rowOffsetForWinnerNames = (int)Math.Pow(2, col + 1) - 1;
            var spaceBetweenWinnerNames = (int)Math.Pow(2, (col + 2));

            for (var row = 0; row < currentRowCount; row++)
            {
                // Add lines between names in the previous column and the next column to the current column
                var rowPositionForLines = rowOffsetForLines + (row * spaceBetweenWinnerNames);
                var matchDrawable = new BracketDrawable(new Label().TextColor, rowSpanForLines);
                var graphicsView = new GraphicsView
                {
                    Drawable = matchDrawable,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };
                this.AddWithSpan(
                    graphicsView,
                    rowPositionForLines,
                    currentColumn,
                    rowSpanForLines
                );

                // Add the winner's name to the next column
                var rowPositionForWinnerNames = rowOffsetForWinnerNames + (row * spaceBetweenWinnerNames);
                // TODO autoscale text?
                var winnerLabel = new Label
                {
                    Text = "TBD",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };
                this.Add(
                    winnerLabel,
                    currentColumn + 1,
                    rowPositionForWinnerNames
                );
            }

            currentRowCount /= 2;
        }
    }
}