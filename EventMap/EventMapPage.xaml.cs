using BruTile.Wms;

namespace CS341Project.EventMap;

public partial class EventMapPage : ContentPage
{
    public EventMapPage()
    {
        InitializeComponent();

        // TODO only shows up the second time the screen is navigated to
        MapControl.Map.Layers.Add(
            Mapsui.Tiling.OpenStreetMap.CreateTileLayer()
        );
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CheckAndRequestLocationPermission();
    }


    private async Task<PermissionStatus> CheckAndRequestLocationPermission()
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (status == PermissionStatus.Granted)
        {
            return status;
        }

        if (status == PermissionStatus.Denied)
        {
            await DisplayAlert(
                "Permission Needed",
                "You need to enable location to see nearby events." +
                "Your location data will not be stored or transmitted in any way.",
                "OK"
            );
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            return status;
        }

        if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
        {
            await DisplayAlert(
                "Permission Rationale",
                "Your location is used to determine which events are nearby. " +
                "Your location data will not be stored or transmitted in any way.",
                "OK"
            );
        }

        status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

        return status;
    }

    private void OnViewAllEventsButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new EventListPage());
    }
}