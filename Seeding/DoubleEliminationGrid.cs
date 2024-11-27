using TOTools.Models.Startgg;

namespace TOTools.Seeding;

/// <summary>
/// A page that displays a double elimination bracket.
/// </summary>
public class DoubleEliminationGrid : Grid
{
    /// <summary>
    /// Creates a double elimination grid.
    /// </summary>
    /// <param name="sets">
    /// The sets of the bracket.
    /// The winner's set must be the last set.
    /// </param>
    /// <param name="color">
    /// The color of the lines and the text in the bracket.
    /// </param>
    public DoubleEliminationGrid(List<Set> sets, Color color)
    {
        FillGridWithPlayers(sets, color);
    }

    /// <summary>
    /// Creates the grid with player sets.
    /// </summary>
    /// <param name="sets">The sets of the bracket.</param>
    /// <param name="color">
    /// The color of the lines and the text in the bracket.
    /// </param>
    private void FillGridWithPlayers(List<Set> sets, Color color)
    {
        var groupedSets = sets.First().Round > -1
            ? sets.GroupBy(set => set.Round).OrderBy(g => g.Key).ToList()
            : sets.GroupBy(set => set.Round).OrderBy(g => -g.Key).ToList();

        // Add the rows
        RowDefinitions.Clear();
        // To ensure the columns after the first have the names centered,
        // an extra row in between names is used.
        // |name--|
        // |      |--name
        // |name--|
        var firstRowSets = groupedSets.First().ToList();
        var numberOfRows = firstRowSets.Count * 2 - 1;
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
        // The extra column is for the final match
        var numberOfColumns = 2 * groupedSets.Count - 1;
        for (var i = 0; i < numberOfColumns; i++)
        {
            ColumnDefinitions.Add(
                new ColumnDefinition { Width = GridLength.Star });
        }

        List<int> previousColumnRows = [];
        for (
            var currentPlayerNumber = 0;
            currentPlayerNumber < firstRowSets.Count;
            currentPlayerNumber++) // Fill the first column with players
        {
            // TODO autoscale text?
            var currentSet = firstRowSets[currentPlayerNumber];
            var playerLabel = new Label
            {
                Text = currentSet.Identifier + ": " + currentSet.Player1 + "vs ." + currentSet.Player2,
                TextColor = color,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            // Handle drag start
            var dragGesture = new DragGestureRecognizer();
            dragGesture.DragStarting += (sender, args) =>
            {
                args.Data.Properties["Set"] = currentSet;
                args.Data.Properties["Label"] = playerLabel;
            };
            playerLabel.GestureRecognizers.Add(dragGesture);

            // Add DropGestureRecognizer
            var dropGesture = new DropGestureRecognizer();
            dropGesture.Drop += (sender, args) =>
            {
                
                if (!args.Data.Properties.TryGetValue("Label", out var sourceLabel)||
                    !args.Data.Properties.TryGetValue("Set", out var set))
                {
                    return;
                }

                (playerLabel.Text, (sourceLabel as Label).Text) = ((sourceLabel as Label).Text, playerLabel.Text);
                currentSet.SwapPlayerWith(set as Set);
            };
            playerLabel.GestureRecognizers.Add(dropGesture);

            this.Add(
                playerLabel,
                0,
                (currentPlayerNumber * 2)
            );
            previousColumnRows.Add(currentPlayerNumber * 2);
        }

        // Fill the winner columns and add the lines in between names
        List<int> currentColumnRows = [];
        var col = 0;
        foreach (var winnerSetGroup in groupedSets.Skip(1))
        {
            var currentWinnerColumn = (2 * col) + 2;
            var row = 0;
            foreach (var currentSet in winnerSetGroup)
            {
                // Add the winner's name to the next column
                int rowPositionForWinnerNames;
                if (previousColumnRows.Count == winnerSetGroup.Count())
                {
                    rowPositionForWinnerNames = previousColumnRows[row];
                }
                else
                {
                    rowPositionForWinnerNames =
                        (previousColumnRows[row * 2] + previousColumnRows[row * 2 + 1]) / 2;
                }

                // TODO autoscale text?
                var winnerLabel = new Label
                {
                    Text = currentSet.Identifier + ": " + currentSet.Player1 + "vs ." + currentSet.Player2,
                    TextColor = color,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };
                this.Add(
                    winnerLabel,
                    currentWinnerColumn,
                    rowPositionForWinnerNames
                );
                currentColumnRows.Add(rowPositionForWinnerNames);

                // Add the lines between the last row and 
                var rowPositionForLines = previousColumnRows.Count == winnerSetGroup.Count()
                    ? previousColumnRows[row]
                    : previousColumnRows[row * 2];
                var rowSpanForLines = previousColumnRows.Count == winnerSetGroup.Count()
                    ? 1
                    : (previousColumnRows[row * 2 + 1] - previousColumnRows[row * 2]) + 1;

                var matchDrawable = new DoubleEliminationDrawable(color, rowSpanForLines);
                var graphicsView = new GraphicsView
                {
                    Drawable = matchDrawable,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };
                this.AddWithSpan(
                    graphicsView,
                    rowPositionForLines,
                    currentWinnerColumn - 1,
                    rowSpanForLines
                );
                row++;
            }

            previousColumnRows = currentColumnRows;
            currentColumnRows = [];
            col++;
        }
    }
}