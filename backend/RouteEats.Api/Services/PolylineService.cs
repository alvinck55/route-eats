namespace RouteEats.Api.Services;

public class PolylineService : IPolylineService
{
    // Google's encoded polyline algorithm:
    // https://developers.google.com/maps/documentation/utilities/polylinealgorithm
    public IReadOnlyList<LatLng> Decode(string encodedPolyline)
    {
        var points = new List<LatLng>();
        int index = 0, lat = 0, lng = 0;

        while (index < encodedPolyline.Length)
        {
            lat += DecodeChunk(encodedPolyline, ref index);
            lng += DecodeChunk(encodedPolyline, ref index);
            points.Add(new LatLng(lat / 1e5, lng / 1e5));
        }

        return points;
    }

    public IReadOnlyList<LatLng> Sample(IReadOnlyList<LatLng> points, int count = 10)
    {
        if (points.Count <= count)
            return points;

        var sampled = new List<LatLng>(count);
        double step = (double)(points.Count - 1) / (count - 1);

        for (int i = 0; i < count; i++)
            sampled.Add(points[(int)Math.Round(i * step)]);

        return sampled;
    }

    private static int DecodeChunk(string encoded, ref int index)
    {
        int result = 0, shift = 0, b;
        do
        {
            b = encoded[index++] - 63;
            result |= (b & 0x1F) << shift;
            shift += 5;
        } while (b >= 0x20);

        return (result & 1) != 0 ? ~(result >> 1) : result >> 1;
    }
}
