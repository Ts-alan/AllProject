using CCP.WebApi.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;

[assembly: OwinStartup(typeof(CCP.WebApi.Startup))]
namespace CCP.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            ConfigureOAuth(app);
            WebApiConfig.Register(config);
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);
        }
        public void ConfigureOAuth(IAppBuilder app)
        {
            var OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new SimpleAuthorizationServerProvider(), 
                RefreshTokenProvider = new SimpleRefreshTokenProvider()
            };

            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }

    }
} 