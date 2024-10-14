﻿namespace Identity.API.Configuration;

public class Config
{
    // ApiResources define the apis in your system
    public static IEnumerable<ApiResource> GetApis()
    {
            return new List<ApiResource>
            {
                new("statistics", "Statistics Service")
                {
                    Scopes = new[] { "statistics" }
                }
            };
        }

    // ApiScope is used to protect the API 
    //The effect is the same as that of API resources in IdentityServer 3.x
    public static IEnumerable<ApiScope> GetApiScopes()
    {
            return new List<ApiScope>
            {
                new("statistics", "Statistics Service")
            };
        }

    // Identity resources are data like user ID, name, or email address of a user
    // see: http://docs.identityserver.io/en/release/configuration/resources.html
    public static IEnumerable<IdentityResource> GetResources()
    {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

    // client want to access resources (aka scopes)
    public static IEnumerable<Client> GetClients(IConfiguration configuration)
    {
            return new List<Client>
            {
                new()
                {
                    ClientId = "webapp",
                    ClientName = "WebApp Client",
                    ClientSecrets = new List<Secret>
                    {
                        new("secret".Sha256())
                    },
                    ClientUri = $"{configuration["WebAppClient"]}", // public uri of the client
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowAccessTokensViaBrowser = false,
                    RequireConsent = false,
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RequirePkce = false,
                    RedirectUris = new List<string>
                    {
                        $"{configuration["WebAppClient"]}/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        $"{configuration["WebAppClient"]}/signout-oidc"
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "statistics",
                    },
                    AccessTokenLifetime = 2 * 60 * 60, // 2 hours
                    IdentityTokenLifetime = 2 * 60 * 60 // 2 hours
                }
            };
        }
}