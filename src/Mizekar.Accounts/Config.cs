using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using Mizekar.Accounts.Constants;

namespace Mizekar.Accounts
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new ApiResource[]
            {
                new ApiResource("storage", "Mizekar Storage Api"){ UserClaims = { JwtClaimTypes.Role, JwtClaimTypes.PhoneNumber }},
                new ApiResource("business", "Mizekar Business Api"){ UserClaims = { JwtClaimTypes.Role, JwtClaimTypes.PhoneNumber }},
                new ApiResource("shop", "Mizekar Shop Api"){ UserClaims = { JwtClaimTypes.Role, JwtClaimTypes.PhoneNumber }},
                new ApiResource("mvc", "mvc site"),
                new ApiResource("api1", "web api client"),
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "shop-mobile",
                    ClientName = "Shop Mobile",
                    AllowedGrantTypes = new List<string> { AuthConstants.GrantType.PhoneNumberToken},
                    ClientSecrets = {new Secret("00PcCMVwUGdb5weDo9FOOrYclGif7SJAFM3oXQGelhy4KQ5f8M3RMuTqeg".Sha256())},
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "shop","storage"
                    },
                    AllowOfflineAccess = true,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    AccessTokenLifetime = 3600,
                    //AllowedCorsOrigins = new List<string>()
                    //{
                    //    "http://localhost:80",
                    //    ""
                    //}
                },
                // client credentials flow client
                new Client
                {
                    ClientId = "client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedScopes = { "api1" }
                },

                // MVC client using hybrid flow
                new Client
                {
                    ClientId = "business",
                    ClientName = "Business app",
                    RequireConsent = false,

                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    RedirectUris = { "https://business.mizekar.com/signin-oidc" },
                    FrontChannelLogoutUri = "https://business.mizekar.com/signout-oidc",
                    PostLogoutRedirectUris = { "https://business.mizekar.com/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "business" }
                },

                // MVC client using hybrid flow
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "mvc site",
                    RequireConsent = false,

                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    RedirectUris = { "https://localhost:44392/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:44392/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:44392/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "mvc" }
                },

                // SPA client using implicit flow
                new Client
                {
                    ClientId = "spa",
                    ClientName = "SPA Client",
                    ClientUri = "http://identityserver.io",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris =
                    {
                        "http://localhost:5002/index.html",
                        "http://localhost:5002/callback.html",
                        "http://localhost:5002/silent.html",
                        "http://localhost:5002/popup.html",
                    },

                    PostLogoutRedirectUris = { "http://localhost:5002/index.html" },
                    AllowedCorsOrigins = { "http://localhost:5002" },

                    AllowedScopes = { "openid", "profile", "api1" }
                }
            };
        }
    }
}
