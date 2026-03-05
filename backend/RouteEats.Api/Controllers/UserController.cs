using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteEats.Api.Data;
using RouteEats.Api.Models.DTOs;
using RouteEats.Api.Models.Entities;

namespace RouteEats.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/user")]
public class UserController(AppDbContext db) : ControllerBase
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet("saved-routes")]
    public async Task<ActionResult<IEnumerable<SavedRoute>>> GetSavedRoutes()
    {
        var routes = await db.SavedRoutes
            .Where(r => r.UserId == UserId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return Ok(routes);
    }

    [HttpPost("saved-routes")]
    public async Task<ActionResult<SavedRoute>> SaveRoute([FromBody] RouteSuggestionsRequest request)
    {
        var route = new SavedRoute
        {
            UserId = UserId,
            Origin = request.Origin,
            Destination = request.Destination,
            Categories = request.Categories,
        };

        db.SavedRoutes.Add(route);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSavedRoutes), route);
    }
}
