using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using TOTools.Models;

namespace TOTools.Scheduler;

public partial class ReportMatchPopup : Popup
{
    private readonly SchedulerBusinessLogic _schedulerBusinessLogic;
    private readonly Match _match;
    private string? _winner;
    private string _eventName;
    
    public ReportMatchPopup(
        string eventName,
        Match match, 
        SchedulerBusinessLogic schedulerBusinessLogic
        )
    {
        InitializeComponent();
        _eventName = eventName;
        _match = match;
        WinnerPicker.Items.Add(_match.Player1);
        WinnerPicker.Items.Add(_match.Player2);
        _schedulerBusinessLogic = schedulerBusinessLogic;
    }

    private void OnOkButtonClicked(object? sender, EventArgs e)
    {
        var errorMessage = "";
        _winner = WinnerPicker.SelectedItem as string;
        if (!int.TryParse(TimeEntry.Text, out var timeInSeconds))
        {
            errorMessage = "Time entry must be an integer.";
        }
        else if (_winner == null)
        {
            errorMessage = "You must select a winner.";
        }
        
        if (errorMessage != "")
        {
            var errorMessageToast = Toast.Make(errorMessage);
            errorMessageToast.Show();
            return;
        }
        _match.TimeInSeconds = timeInSeconds;
        _schedulerBusinessLogic.ReportMatch(_eventName, _match, _winner!);
        Close();
    }

    private void OnCancelButtonClicked(object? sender, EventArgs e)
    {
        Close();
    }
}