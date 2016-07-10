// --------------------------------------------------------------------------------------------------
//  <copyright file="Startup" company="Flush Arcade Pty Ltd.">
//    Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

[assembly: Microsoft.Owin.OwinStartup(typeof(Chat.Startup))]
namespace Chat
{
    using System;
    using System.Web.Http;
    using System.Net;

    using Microsoft.AspNet.SignalR;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.OAuth;
    using Owin;

    using Chat.Providers;

    /// <summary>
    /// The Startup.
    /// </summary>
    public class Startup
    {
        #region Public Methods

        /// <summary>
        /// Handles WebApi configuration.
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            ConfigureOAuth(app);

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);

            app.UseWebApi(config);
            app.MapSignalR();
        }

        /// <summary>
        /// Configures the OAuth.
        /// </summary>
        /// <param name="app"></param>
        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new AuthorizationServerProvider()
            };

            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions()
            {
                Provider = new OAuthBearerTokenAuthenticationProvider()
            });

        }

        #endregion
    }
}