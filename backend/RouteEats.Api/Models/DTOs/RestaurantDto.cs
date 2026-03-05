namespace RouteEats.Api.Models.DTOs;

public record RestaurantDto(
    string PlaceId,
    string Name,
    string? Address,
    double Latitude,
    double Longitude,
    double? Rating,
    int? UserRatingsTotal,
    string[] Categories,
    bool IsOpen
);
