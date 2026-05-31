using FlightSearch.Api.Data;
using FlightSearch.Api.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, config) =>
    config.ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddControllers();
builder.Services.AddSingleton<IAirportRepository, AirportRepository>();

const string corsPolicy = "AllowFrontend";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseCors(corsPolicy);
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
