using System.Collections.ObjectModel;
using TOTools.Models;
using TOTools.Models.Startgg;

namespace TOTools.Seeding;

/// <summary>
/// An interface for getting a notification for when two sets in a bracket are swapped.
/// </summary>
public interface IOnSetsSwapped
{
    /// <summary>
    /// Called when two sets in a bracket have had their players swapped.
    /// </summary>
    /// <param name="set1">One of the sets that was part of the swap.</param>
    /// <param name="set2">The other set that was part of the swap.</param>
    void Swapped(Set set1, Set set2);
}

/// <summary>
/// A page that displays a double elimination bracket.
/// </summary>
public class DoubleEliminationGrid : Grid
{
    
    private IOnSetsSwapped _onSetsSwapped;

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
    /// <param name="onSetsSwapped">Called when two sets in the bracket are swapped.</param>
    /// <param name="isWinnerBracket">Whether this is a winner bracket.</param>
    public DoubleEliminationGrid(ObservableCollection<Player> allPlayers, List<Set> sets, Color color, IOnSetsSwapped onSetsSwapped, bool isWinnerBracket)
    {
        _onSetsSwapped = onSetsSwapped;
        FillGridWithPlayers(allPlayers, sets, color, isWinnerBracket);
    }

    /// <summary>
    /// Creates the grid with player sets.
    /// </summary>
    /// <param name="sets">The sets of the bracket.</param>
    /// <param name="color">
    ///     The color of the lines and the text in the bracket.
    /// </param>
    /// <param name="isWinner">Whether this is a winner bracket.</param>
    private void FillGridWithPlayers(ObservableCollection<Player> allPlayers, List<Set> sets, Color color, bool isWinner)
    {
        if (sets.Count == 0)
        {
            return;
        }
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
        var maxGroupSize = sets
            .GroupBy(set => set.Round)
            .Max(group => group.Count());
        var numberOfRows = maxGroupSize * 2 - 1;
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
        var numberOfColumns = 2 * groupedSets.Count - 1;
        for (var i = 0; i < numberOfColumns; i++)
        {
            ColumnDefinitions.Add(
                new ColumnDefinition { Width = GridLength.Star });
        }

        List<int> previousColumnRows = [];
        var firstRowSets = groupedSets.First().ToList();
        for (
            var currentPlayerNumber = 0;
            currentPlayerNumber < firstRowSets.Count;
            currentPlayerNumber++) // Fill the first column with players
        {
            // TODO autoscale text?
            var currentSet = firstRowSets[currentPlayerNumber];
            var player1Color = allPlayers
                .First(player => player.StarttggId == currentSet.Player1Id)
                .PlayerRegion.GetRegionColor();
            var player2Color = allPlayers
                .First(player => player.StarttggId == currentSet.Player2Id)
                .PlayerRegion.GetRegionColor();
            var playerLabel = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FormattedText = new FormattedString
                {
                    Spans =
                    {
                        new Span
                        {
                            Text = $"{currentSet.Identifier}: ",
                            TextColor = color
                        },
                        new Span
                        {
                            Text = currentSet.Player1DisplayName,
                            TextColor = player1Color
                        },
                        new Span
                        {
                            Text = " vs. ",
                            TextColor = color 
                        },
                        new Span
                        {
                            Text = currentSet.Player2DisplayName,
                            TextColor = player2Color
                        }
                    }
                }
            };

            // TODO Do not allow dragging if the set has a winner

            if (isWinner)
            {
                // Handle drag start
                var dragGesture = new DragGestureRecognizer();
                dragGesture.DragStarting += (_, args) =>
                {
                    args.Data.Properties["Set"] = currentSet;
                    args.Data.Properties["Label"] = playerLabel;
                };
                playerLabel.GestureRecognizers.Add(dragGesture);

                // Add DropGestureRecognizer
                var dropGesture = new DropGestureRecognizer();
                dropGesture.Drop += (_, args) =>
                {

                    if (!args.Data.Properties.TryGetValue("Label", out var sourceLabel) ||
                        !args.Data.Properties.TryGetValue("Set", out var sourceSet))
                    {
                        return;
                    }

                    if (sourceLabel == null || sourceSet == null || sourceLabel is not Label label ||
                        sourceSet is not Set set)
                    {
                        return;
                    }

                    (playerLabel.FormattedText, label.FormattedText) = (label.FormattedText, playerLabel.FormattedText);
                    currentSet.SwapPlayerWith(set);
                    _onSetsSwapped.Swapped(currentSet, set);
                };
                playerLabel.GestureRecognizers.Add(dropGesture);
            }

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
                var rowRatio = winnerSetGroup.Count() / previousColumnRows.Count;
                if (previousColumnRows.Count == winnerSetGroup.Count())
                {
                    rowPositionForWinnerNames = previousColumnRows[row];
                }
                else if (previousColumnRows.Count < winnerSetGroup.Count())
                {
                    rowPositionForWinnerNames = previousColumnRows[row / rowRatio] + row;
                }
                else
                {
                    rowPositionForWinnerNames =
                        (previousColumnRows[row * 2] + previousColumnRows[row * 2 + 1]) / 2;
                }

                // TODO autoscale text?
                var player1Color = allPlayers
                    .First(player => player.StarttggId == currentSet.Player1Id)
                    .PlayerRegion.GetRegionColor();
                var player2Color = allPlayers
                    .First(player => player.StarttggId == currentSet.Player2Id)
                    .PlayerRegion.GetRegionColor();
                var playerLabel = new Label
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    FormattedText = new FormattedString
                    {
                        Spans =
                        {
                            new Span
                            {
                                Text = $"{currentSet.Identifier}: ",
                                TextColor = color
                            },
                            new Span
                            {
                                Text = currentSet.Player1DisplayName,
                                TextColor = player1Color
                            },
                            new Span
                            {
                                Text = " vs. ",
                                TextColor = color 
                            },
                            new Span
                            {
                                Text = currentSet.Player2DisplayName,
                                TextColor = player2Color
                            }
                        }
                    }
                };
                this.Add(
                    playerLabel,
                    currentWinnerColumn,
                    rowPositionForWinnerNames
                );
                currentColumnRows.Add(rowPositionForWinnerNames);

                // Add the lines between the last row and this row
                if (previousColumnRows.Count < winnerSetGroup.Count())
                {
                    var previousRow = previousColumnRows[row / rowRatio];
                    if (row == previousRow)
                    {
                        const int rowSpanForLines = 1;
                        var matchDrawable = new DoubleEliminationDrawable(color, rowSpanForLines);
                        var graphicsView = new GraphicsView
                        {
                            Drawable = matchDrawable,
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center
                        };
                        this.AddWithSpan(
                            graphicsView,
                            previousRow,
                            currentWinnerColumn - 1,
                            rowSpanForLines
                        );
                    }
                }
                else
                {
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
                }

                row++;
            }

            previousColumnRows = currentColumnRows;
            currentColumnRows = [];
            col++;
        }
    }
}