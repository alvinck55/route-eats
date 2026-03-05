using System.Text.Json;

namespace RouteEats.Api.Services;

public class DirectionsService(HttpClient httpClient, IConfiguration config) : IDirectionsService
{
    private readonly string _apiKey = config["GoogleApis:ApiKey"] ?? throw new InvalidOperationException("Google API key not configured");

    public async Task<string?> GetEncodedPolylineAsync(string origin, string destination)
    {
        var url = $"https://maps.googleapis.com/maps/api/directions/json" +
                  $"?origin={Uri.EscapeDataString(origin)}" +
                  $"&destination={Uri.EscapeDataString(destination)}" +
                  $"&key={_apiKey}";

        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        using var doc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        var root = doc.RootElement;

        if (root.GetProperty("status").GetString() != "OK")
            return null;

        return root
            .GetProperty("routes")[0]
            .GetProperty("overview_polyline")
            .GetProperty("points")
            .GetString();
    }
}
