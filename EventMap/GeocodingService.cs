using System.Text.Json;

namespace TOTools.EventMap;

/// <summary>
/// 
/// </summary>
public class GeocodingService
{
    private static readonly HttpClient HttpClient = new();

    public GeocodingService()
    {
        // Set a custom User-Agent to comply with Nominatim's policy
        HttpClient.DefaultRequestHeaders.Add("User-Agent", "TOTools/1.0 (crbstomp@gmail.com)");
    }

    /// <summary>
    /// Gets the longitude and latitude of a given location by searching for the location via Nominatim's search API.
    /// This method must not be called more than once per second given Nominatim's policy!
    /// </summary>
    /// <param name="location">The location that will be searched for.</param>
    /// <returns></returns>
    private static async Task<(double Latitude, double Longitude)> GetCoordinatesAsync(string location)
    {
        try
        {
            var query = $"https://nominatim.openstreetmap.org/search?q={location},+Wisconsin&format=jsonv2";
            var response = await HttpClient.GetStringAsync(query);
            var json = JsonDocument.Parse(response);

            if (json.RootElement.GetArrayLength() > 0)
            {
                var lat = json.RootElement[0].GetProperty("lat").GetDouble();
                var lon = json.RootElement[0].GetProperty("lon").GetDouble();
                return (lat, lon);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred while trying to get coordinates: {ex.Message}");
        }

        return (0, 0);
    }

    /// <summary>
    /// Gets the latitude and longitude for a list of locations.
    /// </summary>
    /// <param name="locations">The locations to get the coordinates for.</param>
    /// <returns></returns>
    public async Task<List<(string Location, double Latitude, double Longitude)>> GeocodeMultipleLocationsAsync(
        List<string> locations)
    {
        var results = new List<(string Location, double Latitude, double Longitude)>();
        foreach (var location in locations)
        {
            var (latitude, longitude) = await GetCoordinatesAsync(location);

            if (latitude != 0 && longitude != 0)
            {
                results.Add((location, latitude, longitude));
            }

            await Task.Delay(1000); // Respect the rate limit
        }

        return results;
    }
}