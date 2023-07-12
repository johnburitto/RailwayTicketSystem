using FluentValidation;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Infrastructure.Data;
using Infrastructure.Services.Impls;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Globalization;
using System.Reflection;
using WebUI.Constraints;
using WebUI.Providers;

var builder = WebApplication.CreateBuilder(args);

// Add localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewLocalization();

// Add FluentValidations
builder.Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
    .AddFluentValidationClientsideAdapters();

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

// Connect to DB
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQLConnection"));
});

// Dependencies
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddScoped<ITrainService, TrainService>();
builder.Services.AddScoped<ITrainCarService, TrainCarService>();
builder.Services.AddScoped<IPlaceService, PlaceService>();
builder.Services.AddScoped<ITicketService, TicketService>();

// Add Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// OpenId IdentityServer redirection
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "cookie";
    options.DefaultChallengeScheme = "oidc";
})
    .AddCookie("cookie", options =>
    {
        options.Cookie.Name = "idsrv.cookie";
    })
    .AddOpenIdConnect("oidc", options =>
    {
        options.ClientId = builder.Configuration["ClientId"];
        options.ClientSecret = builder.Configuration["ClientSecret"];
        options.Authority = builder.Configuration["SecurityAPIUrl"];
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("role");
        options.Scope.Add(builder.Configuration["ReadScope"] ?? throw new ArgumentNullException(nameof(builder.Configuration)));
        options.Scope.Add(builder.Configuration["WriteScope"] ?? throw new ArgumentNullException(nameof(builder.Configuration)));

        options.GetClaimsFromUserInfoEndpoint = true;
        options.ClaimActions.MapJsonKey("role", "role", "role");
        options.TokenValidationParameters.NameClaimType = "name";
        options.TokenValidationParameters.RoleClaimType = "role";

        options.ResponseType = "code";
        options.UsePkce = true;
        options.ResponseMode = "query";
        options.SaveTokens = true;
    });

// Add HealthChecks
builder.Services.AddHealthChecks();

// Configure route options
builder.Services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap.Add("culture", typeof(LanguageRouteConstraint));
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

// Map HealthChecks
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});


app.MapControllerRoute(
    name: "LocalizedDefault",
    pattern: "{culture:culture}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
