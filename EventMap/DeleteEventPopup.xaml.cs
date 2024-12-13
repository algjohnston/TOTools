using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using TOTools.Models;

namespace TOTools.EventMap;

public partial class DeleteEventPopup : Popup
{
    private readonly EventBusinessLogic _eventBusinessLogic;
    
    public DeleteEventPopup(
        EventBusinessLogic eventBusinessLogic
    )
    {
        InitializeComponent();
        _eventBusinessLogic = eventBusinessLogic;
    }

    private void OnOkButtonClicked(object? sender, EventArgs e)
    {
        var errorMessage = "";
        var eventName = NameEntry.Text;
        Event eventToDelete = null;
        foreach (Event tournament in _eventBusinessLogic.Events)
        {
            if (tournament.EventName == eventName)
            {
                eventToDelete = tournament;
            }
        }
        if (eventToDelete == null)
        {
            errorMessage = "Please enter a valid event name";
        }
        if (errorMessage != "")
        {
            var errorMessageToast = Toast.Make(errorMessage);
            errorMessageToast.Show();
            return;
        }
        _eventBusinessLogic.RemoveEvent(eventToDelete);
        Close();
    }

    private void OnCancelButtonClicked(object? sender, EventArgs e)
    {
        Close();
    }
}