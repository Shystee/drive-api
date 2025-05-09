using Scalar.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.MapScalarApiReference();
}

app.UseHttpsRedirection();

var version = Assembly.GetExecutingAssembly()
	              .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
	              ?.InformationalVersion ??
              "1.0.0";

app.MapHealthChecks("/healthz");
app.MapGet("/version", () => new
	{
		version,
		timestamp = DateTime.UtcNow
	})
	.WithName("GetVersion")
	.WithOpenApi();

app.Run();