using System.Collections.ObjectModel;
using TOTools.Models;

namespace TOTools.Scheduler;

/// <summary>
/// A page with a list of events that are used to come up with a match schedule.
/// </summary>
public partial class SchedulerEventPage : ContentPage
{
    public ObservableCollection<EventLink> Events { get; } = [];

    public SchedulerEventPage()
    {
        InitializeComponent();
        BindingContext = this;
        for (var i = 0; i < 10; i++)
        {
            Events.Add(new EventLink("A", new DateTime(100)));
            Events.Add(new EventLink("B", new DateTime(101)));
            Events.Add(new EventLink("C", new DateTime(102)));
            Events.Add(new EventLink("D", new DateTime(103)));
        }
    }

    private void OnSettingsImageButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new EventPopup());
    }

    private void SubmitButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new MatchSchedulerPage());
    }
}