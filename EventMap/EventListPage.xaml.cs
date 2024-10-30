namespace CS341Project.EventMap;

/// <summary>
/// Alexander Johnston
/// A page that lists all upcoming events.
/// </summary>
public partial class EventListPage : ContentPage
{
    private readonly EventBusinessLogic eventBusinessLogic = new();
    
    public EventListPage()
    {
        InitializeComponent();
        BindingContext = eventBusinessLogic;
    }
}