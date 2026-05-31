using FlightSearch.Api.Controllers;
using FlightSearch.Api.Data;
using FlightSearch.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace FlightSearch.Api.Tests.Controllers;

public class AirportsControllerTests
{
    private readonly Mock<IAirportRepository> _repository = new();
    private readonly AirportsController _controller;

    public AirportsControllerTests()
    {
        _controller = new AirportsController(
            _repository.Object,
            NullLogger<AirportsController>.Instance);
    }

    [Fact]
    public void GetOrigins_ReturnsOk_WithAllOrigins()
    {
        var origins = new List<Airport>
        {
            new("LHR", "London Heathrow"),
            new("CDG", "Paris Charles de Gaulle"),
        };
        _repository.Setup(r => r.GetAllOrigins()).Returns(origins);

        var result = _controller.GetOrigins();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(origins, ok.Value);
    }

    [Fact]
    public void GetDestinations_ValidOrigin_ReturnsOk_WithDestinations()
    {
        var destinations = new List<Airport> { new("CDG", "Paris Charles de Gaulle") };
        _repository.Setup(r => r.GetDestinations("LHR")).Returns(destinations);

        var result = _controller.GetDestinations("LHR");

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(destinations, ok.Value);
    }

    [Fact]
    public void GetDestinations_UnknownOrigin_ReturnsNotFound()
    {
        _repository.Setup(r => r.GetDestinations("XYZ")).Returns((IReadOnlyList<Airport>?)null);

        var result = _controller.GetDestinations("XYZ");

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public void GetDestinations_SingleCharCode_ReturnsBadRequest()
    {
        var result = _controller.GetDestinations("A");

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public void GetDestinations_NonAlphabeticCode_ReturnsBadRequest()
    {
        var result = _controller.GetDestinations("1AB");

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public void GetDestinations_EmptyCode_ReturnsBadRequest()
    {
        var result = _controller.GetDestinations("");

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
}
