namespace TOTools.Seeding;

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
    }
}