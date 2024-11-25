
using CommunityToolkit.Maui.Views;
using TOTools.Models;

namespace TOTools.Seeding;

public partial class PlayerEditorPopup : Popup
{

    public PlayerEditorPopup(Player p)
    {
        InitializeComponent();
        Console.WriteLine("Popup Opened");
        
    }
    
    
}