namespace TOTools.EventMap;

/// <summary>
/// Alexander Johnston
/// A page that lists all upcoming events.
/// </summary>
public partial class EventListPage : ContentPage
{
    
    public EventListPage(EventBusinessLogic eventBusinessLogic)
    {
        InitializeComponent();
        BindingContext = eventBusinessLogic;
    }
}