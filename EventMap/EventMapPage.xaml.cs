using BruTile.Wms;

namespace CS341Project.EventMap;

public partial class EventMapPage : ContentPage
{
    public EventMapPage()
    {
        InitializeComponent();
        
        // Works on Windows 10, but not Android 14
        var mapControl = new Mapsui.UI.Maui.MapControl();
        mapControl.Map.Layers.Add(
            Mapsui.Tiling.OpenStreetMap.CreateTileLayer()
            );
        EventMapGrid.Add(mapControl, row:0);
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CheckAndRequestLocationPermission();
    }


    public async Task<PermissionStatus> CheckAndRequestLocationPermission()
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