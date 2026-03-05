using Microsoft.AspNetCore.Mvc;

namespace RouteEats.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private static readonly string[] Categories =
    [
        "Sweet Treat",
        "Fast Food",
        "Coffee",
        "Sit-down",
        "Meat / BBQ",
    ];

    [HttpGet]
    public ActionResult<string[]> GetCategories() => Ok(Categories);
}
