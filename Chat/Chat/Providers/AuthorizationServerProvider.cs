// --------------------------------------------------------------------------------------------------
//  <copyright file="AuthorizationServerProvider" company="Flush Arcade Pty Ltd.">
//    Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace Chat.Providers
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security.OAuth;

    using System.Security.Claims;
    using System.Threading.Tasks;

    using Chat.Repositories;

    /// <summary>
    /// The authorization server provider.
    /// </summary>
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        #region Public Methods

        /// <summary>
        /// Validates the client's authentication
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        /// <summary>
        /// Grants the resource owner's credentials by checking in the UserManager
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            string userName = null;

            using (AuthenticationRepository authenticationRepository = new AuthenticationRepository())
            {
                IdentityUser user = await authenticationRepository.FindUser(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "Incorrect user name or password");
                    return;
                }

                userName = user.UserName;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("Role", "User"));
            identity.AddClaim(new Claim("UserName", userName));

            context.Validated(identity);
        }

        #endregion
    }
}