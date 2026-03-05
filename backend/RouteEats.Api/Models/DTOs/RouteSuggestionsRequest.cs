namespace RouteEats.Api.Models.DTOs;

public record RouteSuggestionsRequest(
    string Origin,
    string Destination,
    string[] Categories
);
