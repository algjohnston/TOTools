using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using TOTools.Models;

namespace TOTools.Scheduler;

public partial class ReportMatchPopup : Popup
{
    private SchedulerBusinessLogic? _schedulerBusinessLogic;
    private Match match;
    private Player? winner = null;
    private int? timeInSeconds = null;
    public ReportMatchPopup(Match match)
    {
        InitializeComponent();
        this.match = match;
    }

    private void OnOkButtonClicked(object? sender, EventArgs e)
    {
        string? errorMessage = "";
        winner = WinnerPicker.SelectedItem as Player;
        int timeInSeconds;
        if (!int.TryParse(TimeEntry.Text, out timeInSeconds))
        {
            errorMessage = "Time entry must be an integer.";
        }
        else if (winner == null)
        {
            errorMessage = "You must select a winner.";
        }

        IToast errorMessageToast = Toast.Make(errorMessage);
        errorMessageToast.Show();
        if (errorMessage == "")
        {
            match.TimeInSeconds = timeInSeconds;
            _schedulerBusinessLogic.ReportMatch(match, winner);
            Close();
        }
    }

    private void OnCancelButtonClicked(object? sender, EventArgs e)
    {
        Close();
    }
}