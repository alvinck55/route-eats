using RouteEats.Api.Models.DTOs;

namespace RouteEats.Api.Services;

public interface IPlacesService
{
    /// <summary>
    /// Searches for places near <paramref name="location"/> matching the given app categories.
    /// Returns deduplicated results across all sample points.
    /// </summary>
    Task<IReadOnlyList<RestaurantDto>> SearchAlongRouteAsync(
        IReadOnlyList<LatLng> samplePoints,
        string[] categories,
        int radiusMeters = 5000);
}
