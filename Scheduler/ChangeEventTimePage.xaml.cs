using TOTools.Models;

namespace TOTools.Scheduler;

/// <summary>
/// An interface that allows updates to event start times to be received.
/// </summary>
public interface INewTimeSubmitted
{
    /// <summary>
    /// Called when the start time of an event changes.
    /// </summary>
    public void NewTimeSubmitted();
}

/// <summary>
/// A page for changing the time of an event.
/// </summary>
public partial class ChangeEventTimePage : ContentPage
{
    public EventMatchGroup SelectedEvent { get; private set; }
    private INewTimeSubmitted _newTimeSubmitted;
   
    /// <summary>
    /// Creates a page to change the time of the given event.
    /// </summary>
    /// <param name="selectedEvent">The event whose time will be updated.</param>
    /// <param name="newTimeSubmitted">The callback that gets called if the time of the event is changed.</param>
    public ChangeEventTimePage(EventMatchGroup selectedEvent, INewTimeSubmitted newTimeSubmitted)
    {
        InitializeComponent();
        SelectedEvent = selectedEvent;
        BindingContext = this;
        _newTimeSubmitted = newTimeSubmitted;
    }

    private void OnSubmitButtonClicked(object? sender, EventArgs e)
    {
        // Change the start time and return
        if (SelectedEvent != null)
        {
            SelectedEvent.StartTime = DateTime.Today.Add(StartTimeEntry.Time);
            _newTimeSubmitted.NewTimeSubmitted();
        }
        Navigation.PopAsync();
    }

    private void OnCancelButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}