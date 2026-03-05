using Microsoft.AspNetCore.Identity;

namespace RouteEats.Api.Models.Entities;

public class AppUser : IdentityUser
{
    public ICollection<SavedRoute> SavedRoutes { get; set; } = [];
}
