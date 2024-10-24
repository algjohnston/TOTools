using System.Collections.ObjectModel;
using CS341Project.Models;

namespace CS341Project.Scheduler;

public partial class SchedulerEventPage : ContentPage
{
    
    public ObservableCollection<EventLink> Events { get; } = [];
    
    public SchedulerEventPage()
    {
        InitializeComponent();
        BindingContext = this;
        for (var i = 0; i < 10; i++)
        {
            Events.Add(new EventLink("A",  new DateTime(100)));
            Events.Add(new EventLink("B" , new DateTime(101)));
            Events.Add(new EventLink("C",  new DateTime(102)));
            Events.Add(new EventLink("D",  new DateTime(103)));
        }
    }

    private void SettingsImageButton_OnClicked(object? sender, EventArgs e)
    {
        Shell.Current.GoToAsync("event_pop_up");
    }
}