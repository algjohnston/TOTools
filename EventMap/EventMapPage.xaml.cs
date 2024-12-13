using CommunityToolkit.Maui.Views;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Layers.AnimatedLayers;
using Mapsui.Nts;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.Widgets;
using Mapsui.Widgets.ButtonWidgets;
using Mapsui.Widgets.ScaleBar;
using TOTools.Models;
using Color = Mapsui.Styles.Color;

namespace TOTools.EventMap;

/**
 * A page with pins on a map, where each pin is an event.
 */
public partial class EventMapPage : ContentPage
{
    private EventBusinessLogic? _eventBusinessLogic;
    private WritableLayer eventLayer = new() { Style = null };
    public EventMapPage()
    {
        InitializeComponent();
        HandlerChanged += OnHandlerChanged;
    }
    
    private static readonly IStyle pointStyle = new SymbolStyle {
        SymbolScale = 0.5d,
        Fill = new(Color.Orange)
    };

    private async void OnHandlerChanged(object? sender, EventArgs e)
    {
        _eventBusinessLogic ??= Handler?.MauiContext?.Services
            .GetService<EventBusinessLogic>();
        if (_eventBusinessLogic == null)
        {
            return;
        }

        // Need to wait for the load task to ensure the events are loaded
        await _eventBusinessLogic.LoadTask;
        BindingContext = _eventBusinessLogic;
        SetUpMap();
    }

    /// <summary>
    /// Create the map, and centers it on Madison, WI.
    /// Adds a pin for each event to the map.
    /// </summary>
    private void SetUpMap()
    {
        // TODO sometimes does not load 

        // Add the map layer
        var map = MapControl.Map;
        map.Layers.Add(
            Mapsui.Tiling.OpenStreetMap.CreateTileLayer()
        );

        // Shows distance scale 
        map.Widgets.Add(
            new ScaleBarWidget(map)
            {
                TextAlignment = Alignment.Center,
                HorizontalAlignment = Mapsui.Widgets.HorizontalAlignment.Center,
                VerticalAlignment = Mapsui.Widgets.VerticalAlignment.Top
            });
        // Shows zoom buttons
        map.Widgets.Add(new ZoomInOutWidget { Margin = new MRect(20, 40) });

        // Zoom to Madison, WI
        map.CRS = "EPSG:3857";
        var madisonWI = new MPoint(-89.401230, 43.073051);
        var sphericalMercatorCoordinate = SphericalMercator.FromLonLat(
            madisonWI.X,
            madisonWI.Y
        ).ToMPoint();
        map.Navigator.CenterOnAndZoomTo(
            sphericalMercatorCoordinate,
            resolution: 1920,
            500,
            Mapsui.Animations.Easing.CubicOut
        );

        map.Layers.Add(eventLayer);
        UpdateEvents();
    }

    private void UpdateEvents()
    {
        eventLayer.Clear();
        foreach (Event eventToAdd in _eventBusinessLogic.Events)
        {
            eventLayer.Add(CreatePoint(eventToAdd));
        }
    }

    private PointFeature CreatePoint(Event eventToAdd)
    {
        MPoint point = new(eventToAdd.Longitude, eventToAdd.Latitude);
        point = SphericalMercator.FromLonLat(point.X, point.Y).ToMPoint();

        PointFeature feature = new(point);
        feature.Styles.Add(pointStyle);
        return feature;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        CheckAndRequestLocationPermission();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }

    /// <summary>
    /// Checks if there is location permission.
    /// If not, asks for it or shows the rationale for why it is needed.
    /// </summary>
    private async Task CheckAndRequestLocationPermission()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
        switch (status)
        {
            case PermissionStatus.Granted:
                return;
            case PermissionStatus.Denied:
                await DisplayAlert(
                    "Permission Needed",
                    "You need to enable location to see nearby events." +
                    "Your location data will not be stored or transmitted in any way.",
                    "OK"
                );
                await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                return;
            case PermissionStatus.Unknown:
            case PermissionStatus.Disabled:
            case PermissionStatus.Restricted:
            case PermissionStatus.Limited:
                break;
            default:
                throw new ArgumentOutOfRangeException();
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
        await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
    }

    private void OnViewAllEventsButtonClicked(object? sender, EventArgs e)
    {
        Navigation.PushAsync(new EventListPage());
    }

    private async void OnAddEventButtonClicked(object? sender, EventArgs e)
    {
            await this.ShowPopupAsync(
                new AddEventPopup(
                    _eventBusinessLogic
                )
            );
    }
    
}