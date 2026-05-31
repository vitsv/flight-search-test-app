using FlightSearch.Api.Models;

namespace FlightSearch.Api.Data;

public interface IAirportRepository
{
    IReadOnlyList<Airport> GetAllOrigins();
    IReadOnlyList<Airport>? GetDestinations(string originCode);
}
