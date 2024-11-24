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

    private async void OnHandlerChanged(object? sender, EventArgs e)
    {
        _eventBusinessLogic ??= Handler?.MauiContext?.Services
            .GetService<EventBusinessLogic>();
        if (_eventBusinessLogic == null)
        {
            return;
        }

        // Need to wait for the load task to ensure the events are loaded
        await _eventBusinessLogic.LoadTask;
        BindingContext = _eventBusinessLogic;
    }
}