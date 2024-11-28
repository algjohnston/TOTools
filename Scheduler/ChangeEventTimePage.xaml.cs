using TOTools.Models;

namespace TOTools.Scheduler;

public interface INewTimeSubmitted
{
    public void NewTimeSubmitted();
}

public partial class ChangeEventTimePage : ContentPage
{
    public EventMatchGroup SelectedEvent { get; private set; }
    private INewTimeSubmitted _newTimeSubmitted;
    public ChangeEventTimePage(EventMatchGroup selectedEvent, INewTimeSubmitted newTimeSubmitted)
    {
        InitializeComponent();
        SelectedEvent = selectedEvent;
        BindingContext = this;
        _newTimeSubmitted = newTimeSubmitted;
    }

    private void OnSubmitButtonClicked(object? sender, EventArgs e)
    {
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