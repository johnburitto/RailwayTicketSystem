using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add HealthChecks
builder.Services.AddHealthChecksUI().AddInMemoryStorage();

// Serilog configuration
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithExceptionDetails()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(builder.Configuration["ElasticConfiguration:Uri"] ?? throw new ArgumentNullException("ElasticConfiguration:Uri")))
    {
        IndexFormat = $"railwayticketssystem-{Assembly.GetExecutingAssembly().GetName().Name?.ToLower().Replace(".", "-")}-{builder.Environment.EnvironmentName?.ToLower().Replace(".", "-")}-logs-{DateTime.UtcNow:yyyy-MM}",
        AutoRegisterTemplate = true,
        NumberOfShards = 2,
        NumberOfReplicas = 1
    })
    .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName ?? throw new ArgumentNullException(nameof(builder.Environment.EnvironmentName)))
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

// Add Serilog
builder.Host.UseSerilog();

var app = builder.Build();

// Map HealthChecks
app.MapHealthChecksUI();

app.Run();
