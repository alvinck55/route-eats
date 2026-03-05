namespace RouteEats.Api.Models.Entities;

public class SavedRoute
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string[] Categories { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public AppUser User { get; set; } = null!;
}
