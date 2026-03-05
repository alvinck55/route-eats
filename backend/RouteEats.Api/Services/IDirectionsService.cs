namespace RouteEats.Api.Services;

public interface IDirectionsService
{
    /// <summary>
    /// Fetches the encoded polyline for a route between origin and destination.
    /// </summary>
    Task<string?> GetEncodedPolylineAsync(string origin, string destination);
}
