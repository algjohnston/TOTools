using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using TOTools.Models;

namespace TOTools.EventMap;

public partial class AddEventPopup : Popup
{
    private readonly EventBusinessLogic _eventBusinessLogic;
    private string _eventName;
    
    public AddEventPopup(
        EventBusinessLogic eventBusinessLogic
    )
    {
        InitializeComponent();
        _eventBusinessLogic = eventBusinessLogic;
    }

    private void OnOkButtonClicked(object? sender, EventArgs e)
    {
        var eventName = NameEntry.Text;
        var eventAddress = AddressEntry.Text;
        var eventLink = LinkEntry.Text;
        var errorMessage = "";
        if (string.IsNullOrWhiteSpace(eventName))
        {
            errorMessage = "Please enter a name.";
        }
        else if (string.IsNullOrWhiteSpace(eventAddress))
        {
            errorMessage = "Please enter a valid address.";
        }
        else if (string.IsNullOrWhiteSpace(eventLink))
        {
            errorMessage = "Please enter a valid link.";
        }
        if (errorMessage != "")
        {
            var errorMessageToast = Toast.Make(errorMessage);
            errorMessageToast.Show();
            return;
        }

        _eventBusinessLogic.AddEvent(eventName, eventAddress, eventLink);
        Close();
    }

    private void OnCancelButtonClicked(object? sender, EventArgs e)
    {
        Close();
    }
}