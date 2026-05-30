var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

const string corsPolicy = "AllowFrontend";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors(corsPolicy);
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
