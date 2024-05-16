using IdentityServer4.Models;
using IdentityServer4;

namespace IdentityServer
{
    public class IdentityServerConfig
    {

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "your-client-id",
                    ClientSecrets = { new Secret("your-client-secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { "http://localhost:5002/signin-oidc" }, // Redirect URI of your client application
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" }, // Post logout redirect URI of your client application
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    RequirePkce = true,
                    AllowPlainTextPkce = false
                }
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("your-api-scope", "Your API Scope")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("your-api-resource", "Your API Resource")
                {
                    Scopes = { "your-api-scope" }
                }
            };
    }
}
