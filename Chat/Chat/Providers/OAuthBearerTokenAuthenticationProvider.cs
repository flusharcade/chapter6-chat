// --------------------------------------------------------------------------------------------------
//  <copyright file="OAuthBearerTokenAuthenticationProvider" company="Flush Arcade Pty Ltd.">
//    Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace Chat.Providers
{
    using System.Threading.Tasks;
    using System;
    using System.Linq;

    using Microsoft.Owin.Security.OAuth;

    /// <summary>
    /// The OAuth bearer token authentication provider.
    /// </summary>
    public class OAuthBearerTokenAuthenticationProvider : OAuthBearerAuthenticationProvider
    {
        #region Public Methods

        /// <summary>
        /// Requests the token.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task RequestToken(OAuthRequestTokenContext context)
        {
            string cookieToken = null;
            string queryStringToken = null;
            string headerToken = null;

            try
            {
                cookieToken = context.OwinContext.Request.Cookies["BearerToken"];
            }
            catch (NullReferenceException)
            {
                System.Diagnostics.Debug.WriteLine("The cookie does not contain the bearer token");
            }

            try
            {
                queryStringToken = context.OwinContext.Request.Query["BearerToken"].ToString();
            }
            catch (NullReferenceException)
            {
                System.Diagnostics.Debug.WriteLine("The query string does not contain the bearer token");
            }

            try
            {
                headerToken = context.OwinContext.Request.Headers["BearerToken"];
            }
            catch (NullReferenceException)
            {
                System.Diagnostics.Debug.WriteLine("The connection header does not contain the bearer token");
            }

            if (!String.IsNullOrEmpty(cookieToken))
                context.Token = cookieToken;

            else if (!String.IsNullOrEmpty(queryStringToken))
                context.Token = queryStringToken;

            else if (!String.IsNullOrEmpty(headerToken))
                context.Token = headerToken;

            return Task.FromResult<object>(null);
        }

        #endregion
    }
}