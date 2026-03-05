using System.Text.Json;
using RouteEats.Api.Models.DTOs;

namespace RouteEats.Api.Services;

public class PlacesService(HttpClient httpClient, IConfiguration config) : IPlacesService
{
    private readonly string _apiKey = config["GoogleApis:ApiKey"] ?? throw new InvalidOperationException("Google API key not configured");

    // Maps app category names → Google Places types
    private static readonly Dictionary<string, string[]> CategoryTypeMap = new(StringComparer.OrdinalIgnoreCase)
    {
        ["sweet treat"] = ["bakery", "cafe", "dessert_shop"],
        ["fast food"]   = ["meal_takeaway", "fast_food_restaurant"],
        ["coffee"]      = ["cafe", "coffee_shop"],
        ["sit-down"]    = ["restaurant"],
        ["meat / bbq"]  = ["restaurant"],
    };

    public async Task<IReadOnlyList<RestaurantDto>> SearchAlongRouteAsync(
        IReadOnlyList<LatLng> samplePoints,
        string[] categories,
        int radiusMeters = 5000)
    {
        var googleTypes = categories
            .SelectMany(c => CategoryTypeMap.TryGetValue(c, out var types) ? types : [])
            .Distinct()
            .ToArray();

        var tasks = samplePoints.Select(point => SearchNearbyAsync(point, googleTypes, radiusMeters, categories));
        var results = await Task.WhenAll(tasks);

        // Deduplicate by place_id
        return results
            .SelectMany(r => r)
            .GroupBy(r => r.PlaceId)
            .Select(g => g.First())
            .ToList();
    }

    private async Task<IEnumerable<RestaurantDto>> SearchNearbyAsync(
        LatLng location,
        string[] googleTypes,
        int radiusMeters,
        string[] appCategories)
    {
        if (googleTypes.Length == 0)
            return [];

        var typeParam = string.Join("|", googleTypes);
        var url = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json" +
                  $"?location={location.Lat},{location.Lng}" +
                  $"&radius={radiusMeters}" +
                  $"&type={typeParam}" +
                  $"&key={_apiKey}";

        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        using var doc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        var root = doc.RootElement;

        if (!root.TryGetProperty("results", out var results))
            return [];

        return results.EnumerateArray().Select(place =>
        {
            var geo = place.GetProperty("geometry").GetProperty("location");
            var openNow = place.TryGetProperty("opening_hours", out var hours)
                && hours.TryGetProperty("open_now", out var open)
                && open.GetBoolean();

            return new RestaurantDto(
                PlaceId: place.GetProperty("place_id").GetString()!,
                Name: place.GetProperty("name").GetString()!,
                Address: place.TryGetProperty("vicinity", out var v) ? v.GetString() : null,
                Latitude: geo.GetProperty("lat").GetDouble(),
                Longitude: geo.GetProperty("lng").GetDouble(),
                Rating: place.TryGetProperty("rating", out var r) ? r.GetDouble() : null,
                UserRatingsTotal: place.TryGetProperty("user_ratings_total", out var u) ? u.GetInt32() : null,
                Categories: appCategories,
                IsOpen: openNow
            );
        });
    }
}
