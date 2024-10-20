using CommunityToolkit.Mvvm.Messaging;

namespace CS341Project.Seeding;

public partial class SeedingShell : Shell
{
    public SeedingShell()
    {
        InitializeComponent();
    }

    protected override bool OnBackButtonPressed()
    {
        if (Navigation.NavigationStack.Count > 1)
        {
            Navigation.PopAsync();
            return true;
        }

        WeakReferenceMessenger.Default.Send(new BackPressedMessage());
        return false;
    }
}