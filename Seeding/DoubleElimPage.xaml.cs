﻿namespace CS341Project.Seeding;

public partial class DoubleElimPage : ContentPage
{
    public DoubleElimPage()
    {
        InitializeComponent();
        var screenHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
        var screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        
        WinnersBracket.HeightRequest = screenHeight;
        WinnersBracket.WidthRequest = screenWidth;
        WinnersBracket.Content = new DoubleElimGrid();
        
        LosersBracket.HeightRequest = screenHeight;
        LosersBracket.WidthRequest = screenWidth;
        LosersBracket.Content = new DoubleElimGrid();
        
    }
}