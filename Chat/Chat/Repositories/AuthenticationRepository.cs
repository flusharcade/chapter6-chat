// --------------------------------------------------------------------------------------------------
//  <copyright file="AuthenticationRepository" company="Flush Arcade Pty Ltd.">
//    Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace Chat.Repositories
{
    using System;
    using System.Threading.Tasks;
    
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using Chat.Models;

    /// <summary>
    /// The authentication repository.
    /// </summary>
    public class AuthenticationRepository : IDisposable
    {
        #region Private Properties

        /// <summary>
        /// The authentication context.
        /// </summary>
        private AuthenticationContext authenticationContext;

        /// <summary>
        /// The user manager
        /// </summary>
        private UserManager<IdentityUser> userManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new authentication repository.
        /// </summary>
        public AuthenticationRepository()
        {
            authenticationContext = new AuthenticationContext();
            userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(authenticationContext));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Registers the user in the UserManager
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            IdentityUser newUser = new IdentityUser()
            {
                UserName = userModel.Username
            };

            var foundUser = await userManager.FindByNameAsync(newUser.UserName);
            if (foundUser != null)
            {
                await userManager.RemovePasswordAsync(foundUser.Id);
                return await userManager.AddPasswordAsync(foundUser.Id, userModel.Password);
            }
            else
            {
                return await userManager.CreateAsync(newUser, userModel.Password);
            }
        }

        /// <summary>
        /// Finds the user from the UserManager.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            return await userManager.FindAsync(userName, password);
        }

        /// <summary>
        /// Disposes the repository items.
        /// </summary>
        public void Dispose()
        {
            authenticationContext.Dispose();
            userManager.Dispose();
        }

        #endregion
    }
}