﻿using CS341Project.AppEntry;

namespace CS341Project;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // TODO the try catch is for testing the rest of the functionality later
        // since crash messages are not propagated anywhere I can find. 
        try
        {
            MainPage = new NavigationPage(new TitlePage());
        }
        catch (Exception exception)
        {
            MainPage?.DisplayAlert("Error", exception.Message, "OK");
        }
    }
}