using System.Text.Json;

namespace TOTools.EventMap;

public class GeocodingService
{
    private static readonly HttpClient HttpClient = new();

    public GeocodingService()
    {
        // Set a custom User-Agent to comply with Nominatim's policy
        HttpClient.DefaultRequestHeaders.Add("User-Agent", "TOTools/1.0 (crbstomp@gmail.com)");
    }

    public async Task<(double Latitude, double Longitude)> GetCoordinatesAsync(string location)
    {
        try
        {
            string query = $"https://nominatim.openstreetmap.org/search?q={location},+Wisconsin&format=jsonv2";
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
            Console.WriteLine($"Error occurred while geocoding: {ex.Message}");
        }

        return (0, 0);
    }
    
    public async Task<List<(string Location, double Latitude, double Longitude)>> GeocodeMultipleLocationsAsync(List<string> locations)
    {
        var results = new List<(string Location, double Latitude, double Longitude)>();
        var geocodingService = new GeocodingService();

        foreach (var location in locations)
        {
            var (latitude, longitude) = await geocodingService.GetCoordinatesAsync(location);
        
            if (latitude != 0 && longitude != 0)
            {
                results.Add((location, latitude, longitude));
            }

            await Task.Delay(1000); // Respect the rate limit
        }

        return results;
    }
    
}