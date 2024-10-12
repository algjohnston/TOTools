namespace CS341Project;

public partial class DoubleElimPage : ContentPage
{
    public DoubleElimPage()
    {
        InitializeComponent();
        FillGridWithPlayers();
        
    }
    
    private void FillGridWithPlayers()
    {
        List<string> players = [
            "A", "B", "C", "D", "A", "B", "C", "D", "A", "B", "C", "D", "A", "B", "C", "D", "A", "B", "C", "D", "A", "B", "C", "D", "A", "B", "C", "D", "A", "B", "C", "D"
        ];
        int numPlayers = players.Count;
        
        // Add the rows and columns
        GridDoubleElim.RowDefinitions.Clear();
        int numRows = 2 * numPlayers;
        for (int i = 0; i < numRows; i++)
        {
            GridDoubleElim.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
        }
        
        GridDoubleElim.ColumnDefinitions.Clear();
        int numColumns = 2 * (int)Math.Log(numPlayers, 2) + 1;
        for (int i = 0; i < numColumns; i++)
        {
            GridDoubleElim.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        }
        
        // Fill first column with players
        for (int i = 0; i < numPlayers; i++)
        {
            // TODO autoscale?
            var playerLabel = new Label
            {
                Text = players[i], 
                HorizontalOptions = LayoutOptions.Center, 
                VerticalOptions = LayoutOptions.Center
            };
            // To ensure the columns after this have the names centered
            // an extra row in between names is used which is why it is i * 2.
            // |name--|
            // |      |--name
            // |name--|
            GridDoubleElim.Add(playerLabel, 0, (i * 2));
        }

        // Fill the winner columns
        // Only fill every other column since the columns in between have the lines connecting players
        //          v--------- column with lines 
        // |  0  |  1  |  2  |
        // | name --|
        // |        |-- name |
        // | name --|
        int currentRowCount = numPlayers;
        for (int col = 2; col < numColumns; col += 2) 
        {
            int numNamesForNextRow = (int)Math.Ceiling(currentRowCount / 2.0);
            for (int i = 0; i < numNamesForNextRow; i++)
            {
                // TODO autoscale?
                var winnerLabel = new Label
                {
                    Text = "TBD", 
                    HorizontalOptions = LayoutOptions.Center, 
                    VerticalOptions = LayoutOptions.Center
                };
                
                int rowPosition = (int)Math.Pow(2, (col / 2)) - 1 + (i * (int)Math.Pow(2, (col / 2) + 1));
                
                // (col % 2) is to ensure centering
                GridDoubleElim.Add(winnerLabel, col, rowPosition + (col % 2));
            }
            currentRowCount = numNamesForNextRow;
        }

        // Fill every other column with the lines
        currentRowCount = numPlayers / 2;
        for (int col = 1; col < numColumns; col += 2)
        {
            for (int row = 0; row < currentRowCount; row++)
            {
                var rowPosition =  (int)Math.Pow(2, ((col - 1) / 2)) - 1 + (row * (int)(Math.Pow(2, ((col + 1) / 2) + 1)));
                var rowSpan = (int)Math.Pow(2, ((col + 1) / 2)) + 1;
                var matchDrawable = new DoubleElimDrawable(new Label().TextColor, rowSpan);
                var graphicsView = new GraphicsView
                {
                    Drawable = matchDrawable,
                    HorizontalOptions = LayoutOptions.Center, 
                    VerticalOptions = LayoutOptions.Center
                };
                GridDoubleElim.AddWithSpan(graphicsView, rowPosition, col, rowSpan, 1);
            }

            currentRowCount /= 2;
        }
    }
}