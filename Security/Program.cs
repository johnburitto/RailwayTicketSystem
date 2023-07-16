using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Security.Configurations;
using Security.Constraints;
using Security.Data;
using Security.Entities;
using Security.Providers;
using Security.Seeders;
using Security.Services.Impls;
using Security.Services.Interfaces;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Add MVC
builder.Services.AddControllersWithViews()
    .AddViewLocalization();

// Configure Serilog
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

// Connect to DB
builder.Services.AddDbContext<SecurityDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQLConnection")));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CORSPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add ASP Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password = new PasswordOptions
    {
        RequireDigit = true,
        RequiredLength = 6,
        RequireLowercase = true,
        RequireUppercase = false,
        RequireNonAlphanumeric = false
    };
})
    .AddEntityFrameworkStores<SecurityDbContext>()
    .AddDefaultTokenProviders();

// For API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependencies
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();

// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add IdentityServer
builder.Services.AddRazorPages();
builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;

    options.EmitStaticAudienceClaim = true;
})
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = db => db.UseSqlServer(builder.Configuration.GetConnectionString("MSSQLConnection"),
            assembly => assembly.MigrationsAssembly(typeof(Config).Assembly.GetName().Name));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = db => db.UseSqlServer(builder.Configuration.GetConnectionString("MSSQLConnection"),
            assembly => assembly.MigrationsAssembly(typeof(Config).Assembly.GetName().Name));
    })
    .AddAspNetIdentity<User>();

builder.Services.ConfigureApplicationCookie(options =>
{;
    options.Cookie.Name = "idsrv";
    options.LoginPath = "/uk/Auth/Login";

    options.Cookie.SameSite = SameSiteMode.None;
});

builder.Services.AddAntiforgery(options =>
{
    options.Cookie.Name = "idsrv.antiforgery";
});

// Add HealthChecks
builder.Services.AddHealthChecks();

// Configure route options
builder.Services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap.Add("culture", typeof(LanguageRouteConstraint));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure localization
var supportedCultures = new[]
            {
                new CultureInfo("uk"),
                new CultureInfo("en")
            };

app.UseRequestLocalization(new RequestLocalizationOptions
{
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    RequestCultureProviders = new[]
    {
        new RouteDataRequestCultureProvider
        {
            IndexOfCulture = 1,
            IndexOfUICulture = 1
        }
    }
});

app.UseIdentityServer();

app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

// Map HealthChecks
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapControllerRoute(
    name: "LocalizedDefault",
    pattern: "{culture:culture}/{controller=Auth}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();