using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.Widgets;
using Mapsui.Widgets.ButtonWidgets;
using Mapsui.Widgets.ScaleBar;
using NetTopologySuite.Geometries;

namespace CS341Project.EventMap;

/**
 * A page with pins on a map, where each pin is an event.
 */
public partial class EventMapPage : ContentPage
{
    public EventMapPage()
    {
        InitializeComponent();

        // TODO sometimes does not load 
        var map = MapControl.Map;
        map.Layers.Add(
            Mapsui.Tiling.OpenStreetMap.CreateTileLayer()
        );
        map.Widgets.Add(
            new ScaleBarWidget(map)
            {
                TextAlignment = Alignment.Center,
                HorizontalAlignment = Mapsui.Widgets.HorizontalAlignment.Center,
                VerticalAlignment = Mapsui.Widgets.VerticalAlignment.Top
            });
        map.Widgets.Add(new ZoomInOutWidget { Margin = new MRect(20, 40) });
        map.CRS = "EPSG:3857";
        var madisonWI = new MPoint(-89.401230, 43.073051);
        var sphericalMercatorCoordinate = SphericalMercator.FromLonLat(
            madisonWI.X,
            madisonWI.Y
        ).ToMPoint();
        map.Navigator.CenterOnAndZoomTo(
            sphericalMercatorCoordinate,
            resolution: 1000,
            500, 
            Mapsui.Animations.Easing.CubicOut
        );
        
        var layer = new GenericCollectionLayer<List<IFeature>>
        {
            Style = SymbolStyles.CreatePinStyle()
        };
        map.Layers.Add(layer);
        map.Info += (s, e) =>
        {
            if (e.MapInfo?.WorldPosition == null) return;

            // Add a point to the layer using the Info position
            layer?.Features.Add(new GeometryFeature
            {
                Geometry = new NetTopologySuite. Geometries.Point(
                    e.MapInfo.WorldPosition.X, 
                    e.MapInfo.WorldPosition.Y
                    )
            });
            layer?.DataHasChanged();
        };
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

        return await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
    }

    private void OnViewAllEventsButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new EventListPage());
    }
}