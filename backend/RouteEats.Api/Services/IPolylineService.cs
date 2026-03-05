namespace RouteEats.Api.Services;

public record LatLng(double Lat, double Lng);

public interface IPolylineService
{
    /// <summary>
    /// Decodes a Google-encoded polyline string into a list of coordinates.
    /// </summary>
    IReadOnlyList<LatLng> Decode(string encodedPolyline);

    /// <summary>
    /// Samples approximately <paramref name="count"/> evenly-spaced points along the decoded path.
    /// </summary>
    IReadOnlyList<LatLng> Sample(IReadOnlyList<LatLng> points, int count = 10);
}
