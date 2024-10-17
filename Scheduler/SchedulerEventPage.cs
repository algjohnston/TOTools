using System.Collections.ObjectModel;
using CS341Project.Models;

namespace CS341Project.Scheduler;

public partial class SchedulerEventPage : ContentPage
{
    
    public ObservableCollection<Event> Events { get; } = [];
    
    public SchedulerEventPage()
    {
        InitializeComponent();
        BindingContext = this;
        for (var i = 0; i < 10; i++)
        {
            Events.Add(new Event("A",  new DateTime(100)));
            Events.Add(new Event("B" , new DateTime(101)));
            Events.Add(new Event("C",  new DateTime(102)));
            Events.Add(new Event("D",  new DateTime(103)));
        }
    }

    private void SettingsImageButton_OnClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new EventPopup());
    }
}