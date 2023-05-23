using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using Microsoft.EntityFrameworkCore;
using Security.Configurations;
using Serilog;

namespace Security.Seeders
{
    public class SeedData
    {
        public static void EnsureSeedData(WebApplication app)
        {
            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetService<PersistedGrantDbContext>()?.Database.Migrate();

                var context = scope.ServiceProvider.GetService<ConfigurationDbContext>();

                EnsureSeedData(context);
            }
        }

        private static void EnsureSeedData(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                foreach (var client in Config.Clients)
                {
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
                Log.Information("Clients being populated");
            }
            else
            {
                Log.Information("Clients already populated");
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var identityResource in Config.IdentityResources)
                {
                    context.IdentityResources.Add(identityResource.ToEntity());
                }

                context.SaveChanges();
                Log.Information("IdentityResources being populated");
            }
            else
            {
                Log.Information("IdentityResources already populated");
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var apiScope in Config.ApiScopes)
                {
                    context.ApiScopes.Add(apiScope.ToEntity());
                }

                context.SaveChanges();
                Log.Information("ApiScopes being populated");
            }
            else
            {
                Log.Information("ApiScopes already populated");
            }

            if (!context.ApiResources.Any())
            {
                foreach (var apiResource in Config.ApiResources)
                {
                    context.ApiResources.Add(apiResource.ToEntity());
                }

                context.SaveChanges();
                Log.Information("ApiResources being populated");
            }
            else
            {
                Log.Information("ApiResources already populated");
            }

            if (!context.IdentityProviders.Any())
            {
                context.IdentityProviders.Add(new OidcProvider
                {
                    Scheme = "demoidsrv",
                    DisplayName = "IdentityServer",
                    Authority = "https://demo.duendesoftware.com",
                    ClientId = "login",
                }.ToEntity());

                context.SaveChanges();
                Log.Debug("OIDC IdentityProviders being populated");
            }
            else
            {
                Log.Debug("OIDC IdentityProviders already populated");
            }
        }
    }
}
