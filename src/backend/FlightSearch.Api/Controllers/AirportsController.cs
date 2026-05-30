using FlightSearch.Api.Data;
using FlightSearch.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlightSearch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AirportsController(AirportRepository repository, ILogger<AirportsController> logger) : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyList<Airport>> GetOrigins()
    {
        var origins = repository.GetAllOrigins();
        logger.LogInformation("Returning {Count} origin airports", origins.Count);
        return Ok(origins);
    }

    [HttpGet("destinations/{origin}")]
    public ActionResult<IReadOnlyList<Airport>> GetDestinations(string origin)
    {
        if (string.IsNullOrWhiteSpace(origin) || origin.Length < 2 || !origin.All(char.IsLetter))
        {
            logger.LogWarning("Invalid origin code: '{Origin}'", origin);
            return BadRequest(new { error = "Origin code must contain only letters and be at least 2 characters long." });
        }

        var destinations = repository.GetDestinations(origin);
        if (destinations is null)
        {
            logger.LogWarning("Origin airport not found: '{Origin}'", origin);
            return NotFound(new { error = $"Origin airport '{origin.ToUpperInvariant()}' was not found." });
        }

        logger.LogInformation("Returning {Count} destinations for origin '{Origin}'", destinations.Count, origin.ToUpperInvariant());
        return Ok(destinations);
    }
}
