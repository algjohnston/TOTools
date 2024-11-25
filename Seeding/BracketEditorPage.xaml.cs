namespace TOTools.Seeding;

/// <summary>
/// A page that displays a bracket for editing.
/// </summary>
public partial class BracketEditorPage : ContentPage
{
    public BracketEditorPage()
    {
        InitializeComponent();

        // Saving this code for later
        var screenHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
        var screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        var winnersBracket = new ContentView
        {
            HeightRequest = screenHeight,
            WidthRequest = screenWidth,
            Content = new DoubleElimGrid()
        };
        // TODO add the players to the DoubleElimGrid and support round robin
    }
}