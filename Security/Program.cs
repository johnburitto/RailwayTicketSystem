using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Security.Configurations;
using Security.Constraints;
using Security.Data;
using Security.Entities;
using Security.Providers;
using Security.Services.Impls;
using Security.Services.Interfaces;
using Serilog;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Add MVC
builder.Services.AddControllersWithViews()
    .AddViewLocalization();

// Configure Serilog
Log.Logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .CreateBootstrapLogger();

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
builder.Services.AddIdentity<User, IdentityRole>()
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
{
    options.LoginPath = "/uk/Auth/Login";
});

// Configure route options
builder.Services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap.Add("culture", typeof(LanguageRouteConstraint));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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

app.UseIdentityServer();

app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "LocalizedDefault",
    pattern: "{culture:culture}/{controller=Auth}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();