using Duende.IdentityServer.Models;
using IdentityModel;
using static Duende.IdentityServer.IdentityServerConstants;

namespace Security.Configurations
{
    public class Config
    {
        public static List<IdentityResource> IdentityResources => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource("role", new[] { "role" })
        };

        public static List<ApiScope> ApiScopes => new List<ApiScope>
        {
            new ApiScope("railwaytickets.read"),
            new ApiScope("railwaytickets.write")
        };

        public static List<ApiResource> ApiResources => new List<ApiResource>
        {
            new ApiResource("railwaytickets")
            {
                Scopes = new List<string> { "railwaytickets.read", "railwaytickets.write" },
                ApiSecrets = new List<Secret> {new Secret("RailWaySecret".Sha256())},
                UserClaims = new List<string> { "role" }
            },
            new ApiResource(
                LocalApi.ScopeName, 
                "Local Api", 
                new[] { JwtClaimTypes.Role }
                )
        };

        public static List<Client> Clients => new List<Client>
        {
            new Client
            {
                ClientId = "m2m.client",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = {new Secret("SuperSecretPassword".Sha256())},

                AllowedScopes = { "railwaytickets.read", "railwaytickets.write" }
            },
            new Client
            {
                ClientId = "interactive",
                RequirePkce = true,
                Enabled = true,

                AllowedGrantTypes = GrantTypes.Code,
                ClientSecrets = {new Secret("SuperSecretPassword".Sha256())},
                RedirectUris = {"https://localhost:7250/signin-oidc"},

                AllowOfflineAccess = true,
                AllowedScopes = {
                    StandardScopes.OpenId,
                    StandardScopes.Profile,
                    "railwaytickets.read",
                    "railwaytickets.write",
                    "role"
                },
            },
            new Client
            {
                ClientId = "postman",
                RequirePkce = true,
                Enabled = true,

                AllowedGrantTypes = GrantTypes.Code,
                ClientSecrets = { new Secret("Johnbur1tto!".Sha256()) },
                RedirectUris = { "https://localhost:7250/signin-oidc" },

                AllowedScopes = {
                    StandardScopes.OpenId,
                    StandardScopes.Profile,
                    "railwaytickets.read",
                    "railwaytickets.write"
                }
            }
        };
    }
}
