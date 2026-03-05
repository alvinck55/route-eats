using Microsoft.AspNetCore.Mvc;
using RouteEats.Api.Models.DTOs;
using RouteEats.Api.Services;

namespace RouteEats.Api.Controllers;

[ApiController]
[Route("api/route")]
public class RouteController(
    IDirectionsService directionsService,
    IPolylineService polylineService,
    IPlacesService placesService) : ControllerBase
{
    [HttpPost("suggestions")]
    public async Task<ActionResult<IReadOnlyList<RestaurantDto>>> GetSuggestions(
        [FromBody] RouteSuggestionsRequest request)
    {
        var encodedPolyline = await directionsService.GetEncodedPolylineAsync(
            request.Origin, request.Destination);

        if (encodedPolyline is null)
            return BadRequest("Could not retrieve route from Google Directions API.");

        var allPoints = polylineService.Decode(encodedPolyline);
        var samplePoints = polylineService.Sample(allPoints, count: 10);

        var suggestions = await placesService.SearchAlongRouteAsync(
            samplePoints, request.Categories);

        return Ok(suggestions);
    }
}
