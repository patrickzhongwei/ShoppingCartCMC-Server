using IdentityModel;
using IdentityServer4.Models;
using ShoppingCartCMC.Shared.Common;
using System.Collections.Generic;


namespace ShoppingCartCMC.STS
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()

                //PW: Don't need below, as we store data into column [AspNetUsers].[PermissionRoles], no extra table created.
                //new IdentityResource("MyPermissionRoles[IdResourceName]", new []{ "MyPermissionRole[ClaimType]" } ) 
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource(StsSetting.ApiResourceName)
                {
                    ApiSecrets =
                    {
                        new Secret(StsSetting.ApiResourceName.Sha256())
                    },
                    Scopes =
                    {
                        "ShoppingCartCMC.WebApi" 
                    },
                    UserClaims = { JwtClaimTypes.Role }
                }
            };
        }


        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientName  = "ShoppingCartCMC.WebApi",
                    ClientId    = "ShoppingCartCMC.WebApi",

                    //PW: AccessTokenType.Jwt is self-contained, it’s a protected data structure with claims and an expiration. 
                    //    no need to communicate with the issuer. This makes JWTs hard to revoke. They will stay valid until they expire.
                    //    Other option is AccessTokenType.Reference, IdentityServer will store the contents of the token in a data store and will only issue a unique identifier for this token back to the client.
                    //    The API receiving this reference must then open a back-channel communication to IdentityServer to validate the token
                    AccessTokenType         = AccessTokenType.Jwt,                 
                    AccessTokenLifetime     = 1200,             
                    IdentityTokenLifetime   = 300,                     
                    RequireClientSecret     = false,
                    AllowedGrantTypes       = GrantTypes.Code, //PW: Implicit is no longer recommended.
                    RequirePkce             = true,
                    AllowAccessTokensViaBrowser = true,       
                    AllowOfflineAccess          = true,       
                    AlwaysIncludeUserClaimsInIdToken = true,   


                    AllowedScopes           = new List<string> {

                        //PW: Below is for Identity resource
                        "openid",
                        "profile",
                        "email",
                         StsSetting.ApiResourceName //PW: Api resource                         
                         //"offline_access" 
                    },
                    RequireConsent = false
                }
            };
        }



    }
}
