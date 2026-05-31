using FlightSearch.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Moq;

namespace FlightSearch.Api.Tests.Data;

public class AirportRepositoryTests : IDisposable
{
    private readonly string _tempDir;
    private readonly Mock<IWebHostEnvironment> _env = new();

    public AirportRepositoryTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(Path.Combine(_tempDir, "Data"));
        _env.Setup(e => e.ContentRootPath).Returns(_tempDir);

        // Copy the shared test fixture into the temp directory
        var fixture = Path.Combine(AppContext.BaseDirectory, "Data", "test-airports.json");
        File.Copy(fixture, Path.Combine(_tempDir, "Data", "airports.json"));
    }

    private AirportRepository CreateRepository() => new(_env.Object);

    [Fact]
    public void GetAllOrigins_ReturnsAllAirports_OrderedByName()
    {
        var repo = CreateRepository();

        var origins = repo.GetAllOrigins();

        Assert.Equal(2, origins.Count);
        Assert.Equal("LHR", origins[0].Code); // "London…" sorts before "Paris…"
        Assert.Equal("CDG", origins[1].Code);
    }

    [Fact]
    public void GetDestinations_ValidCode_ReturnsDestinations()
    {
        var repo = CreateRepository();

        var destinations = repo.GetDestinations("LHR");

        Assert.NotNull(destinations);
        Assert.Equal(2, destinations.Count);
        Assert.Contains(destinations, d => d.Code == "CDG");
        Assert.Contains(destinations, d => d.Code == "JFK");
    }

    [Fact]
    public void GetDestinations_LowercaseCode_ReturnsDestinations()
    {
        var repo = CreateRepository();

        var destinations = repo.GetDestinations("lhr");

        Assert.NotNull(destinations);
        Assert.Equal(2, destinations.Count);
    }

    [Fact]
    public void GetDestinations_UnknownCode_ReturnsNull()
    {
        var repo = CreateRepository();

        var destinations = repo.GetDestinations("XYZ");

        Assert.Null(destinations);
    }

    public void Dispose() => Directory.Delete(_tempDir, recursive: true);
}
