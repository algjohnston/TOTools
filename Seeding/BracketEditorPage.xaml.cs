﻿using TOTools.Models;
using TOTools.Models.Startgg;

namespace TOTools.Seeding;

/// <summary>
/// A page that displays a bracket for editing.
/// </summary>
public partial class BracketEditorPage : ContentPage, IOnSetsSwapped
{
    private SeedingBusinessLogic? _seedingBusinessLogic;

    public BracketEditorPage()
    {
        InitializeComponent();
        HandlerChanged += OnHandlerChanged;
    }

    private async void OnHandlerChanged(object? sender, EventArgs e)
    {
        _seedingBusinessLogic ??= Handler?.MauiContext?.Services
            .GetService<SeedingBusinessLogic>();
        if (_seedingBusinessLogic == null)
        {
            return;
        }

        // Waits for players to load
        await _seedingBusinessLogic.PlayerLoadTask;

        var bracket = _seedingBusinessLogic.GetActiveBracket();
        if (bracket == null)
        {
            return;
        }

        var screenHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
        var screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        if (bracket.BracketType == BracketType.DoubleElimination)
        {
            // Add the double elimination bracket
            var winnerAndLoserBracketSets = Bracket.GetWinnerAndLoserBracketSets(bracket);
            var winnersBracket = new ContentView
            {
                HeightRequest = screenHeight,
                WidthRequest = screenWidth,
                Content = new DoubleEliminationGrid(
                    winnerAndLoserBracketSets.WinnerSets,
                    Colors.AntiqueWhite,
                    this, 
                    true)
            };
            var losersBracket = new ContentView
            {
                HeightRequest = screenHeight,
                WidthRequest = screenWidth,
                Content = new DoubleEliminationGrid(
                    winnerAndLoserBracketSets.LoserSets,
                    Colors.AntiqueWhite,
                    this, false)
            };
            BracketStackLayout.Children.Add(winnersBracket);
            BracketStackLayout.Children.Add(losersBracket);
        }
        else if (bracket.BracketType == BracketType.RoundRobin)
        {
            // Add the round-robin bracket
            var roundRobinBrackets = new ContentView
            {
                HeightRequest = screenHeight,
                WidthRequest = screenWidth,
                Content = new RoundRobinGrid(Bracket.GetRoundRobinSets(bracket), Colors.AntiqueWhite)
            };
            BracketStackLayout.Children.Add(roundRobinBrackets);
        }
    }


    public void Swapped(Set set1, Set set2)
    {
        _seedingBusinessLogic!.UpdateSetInCurrentBracket(set1);
        _seedingBusinessLogic!.UpdateSetInCurrentBracket(set2);
    }
}