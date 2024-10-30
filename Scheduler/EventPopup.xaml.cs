using System.Collections.ObjectModel;

namespace CS341Project.Scheduler;

/// <summary>
/// A list of times that matches start.
/// </summary>
public partial class EventPopup : ContentPage
{

    public ObservableCollection<string> StartTime { get; } = [
        "Player 1 Time", 
        "Player 2 Time", 
        "Player 3 Time", 
        "Player 4 Time", 
        "Player 5 Time",
        "Player 6 Time",
        "Player 7 Time", 
        "Player 8 Time"
    ];

    public EventPopup()
    {
        InitializeComponent();
        BindingContext = this;
    }
}