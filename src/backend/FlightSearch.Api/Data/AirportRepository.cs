using System.Text.Json;
using FlightSearch.Api.Models;

namespace FlightSearch.Api.Data;

public class AirportRepository : IAirportRepository
{
    private record AirportEntry(string Code, string Name, List<Airport> Destinations);

    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    private readonly Dictionary<string, (Airport Origin, IReadOnlyList<Airport> Destinations)> _routes;

    public AirportRepository(IWebHostEnvironment env)
    {
        var path = Path.Combine(env.ContentRootPath, "Data", "airports.json");
        var json = File.ReadAllText(path);
        var entries = JsonSerializer.Deserialize<List<AirportEntry>>(json, JsonOptions)
            ?? throw new InvalidOperationException("airports.json is empty or invalid.");

        _routes = entries.ToDictionary(
            e => e.Code.ToUpperInvariant(),
            e => (new Airport(e.Code.ToUpperInvariant(), e.Name), (IReadOnlyList<Airport>)e.Destinations));
    }

    public IReadOnlyList<Airport> GetAllOrigins() =>
        [.. _routes.Values.Select(r => r.Origin).OrderBy(a => a.Name)];

    public IReadOnlyList<Airport>? GetDestinations(string originCode)
    {
        _routes.TryGetValue(originCode.ToUpperInvariant(), out var entry);
        return entry.Destinations;
    }
}
