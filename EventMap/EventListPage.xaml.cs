namespace TOTools.EventMap;

/// <summary>
/// Alexander Johnston
/// A page that lists all upcoming events.
/// </summary>
public partial class EventListPage : ContentPage
{
    
    private EventBusinessLogic? _eventBusinessLogic;
    
    public EventListPage()
    {
        InitializeComponent();
        HandlerChanged += OnHandlerChanged;
    }

    private void OnHandlerChanged(object? sender, EventArgs e)
    {
        _eventBusinessLogic ??= Handler?.MauiContext?.Services
            .GetService<EventBusinessLogic>();
        BindingContext = _eventBusinessLogic;
    }
    
}