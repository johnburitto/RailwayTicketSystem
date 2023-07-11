using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add HealthChecks
builder.Services.AddHealthChecksUI().AddInMemoryStorage();

// Serilog configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateBootstrapLogger();

// Add Serilog
builder.Host.UseSerilog();

var app = builder.Build();

// Map HealthChecks
app.MapHealthChecksUI();

app.Run();
