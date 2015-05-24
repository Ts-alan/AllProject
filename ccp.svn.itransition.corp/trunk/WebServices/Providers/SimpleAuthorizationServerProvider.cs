using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using CCP.DAL;
using CCP.DAL.DataModels;
using CCP.DAL.Helpers;
using CCP.DAL.Interfaces;
using CCP.WebApi.Helpers;
using CCP.WebApi.Resources;
using CCP.WebApi.Services;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;

namespace CCP.WebApi.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private readonly IRepository<AreaRoleView> _vAreaRoleRepository;

        public SimpleAuthorizationServerProvider()
        {
            _vAreaRoleRepository = _unitOfWork.vAreaRoleRepository;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            Client client = null;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                //Remove the comments from the below line context.SetError, and invalidate context
                //if you want to force sending clientId/secrects once obtain access tokens.
                context.Validated();
                //context.SetError("invalid_clientId", "ClientId should be sent.");
                return;
            }

            using (var service = new UserService())
            {
                client = service.FindClient(context.ClientId);
            }

            if (client == null)
            {
                context.SetError("invalid_clientId", string.Format(Res.InvalidClientId, context.ClientId));
                return;
            }

            if (client.ApplicationType == ApplicationTypes.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", Res.EmptyClientSecret);
                    return;
                }
                else
                {
                    if (client.Secret != TokenHashier.GetHash(clientSecret))
                    {
                        context.SetError("invalid_clientId", Res.InvalidClientSecret);
                        return;
                    }
                }
            }

            if (!client.Active)
            {
                context.SetError("invalid_clientId", Res.InactiveClient);
                return;
            }

            context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var areas = _vAreaRoleRepository.Get().ToList();
            var result = JsonConvert.SerializeObject(AreaModelBuilder.Build(areas));

            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

            if (allowedOrigin == null)
            {
                allowedOrigin = "*";
            }

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            User user = null;

            ClaimsIdentity identity = null;

            var service = new UserService();

            user = await service.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant",Res.AuthorizationError);
                return;
            }


            identity = new ClaimsIdentity(context.Options.AuthenticationType, "name", "role");

            //identity.AddClaim(new Claim("id", user.Id.ToString()));
            identity.AddClaim(new Claim("name", context.UserName));
            identity.AddClaim(new Claim("role", user.Role.RoleName));


            var props = new AuthenticationProperties(new Dictionary<string, string>
            {
                {
                    "as:client_id", context.ClientId ?? string.Empty
                },
                {
                    "user", user.FirstName 
                },
                {
                    "firstName", user.FirstName
                },
                {
                    "lastName", user.LastName
                },
                {
                    "email", user.Email
                },
                {
                    "role", user.Role.RoleName
                },
                {
                    "areas", result
                },
                {
                    "refreshTokenExpireDate", DateTime.Now.AddMinutes(5).ToString()
                },
            });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
            

        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", Res.InvalidRefreshToken);
                return Task.FromResult<object>(null);
            }
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            //newIdentity.AddClaim(new Claim("newClaim", "newValue"));
            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }
    }
}