// --------------------------------------------------------------------------------------------------
//  <copyright file="AccountController" company="Flush Arcade Pty Ltd.">
//    Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace Chat.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Security;
    using System.Web.Http;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;

    using Chat.Models;
    using Chat;
    using Chat.Repositories;

    /// <summary>
    /// The account controller.
    /// </summary>
    public class AccountController : ApiController
    {
        #region Private Properties

        /// <summary>
        /// The account respository.
        /// </summary>
        private AuthenticationRepository authenticationRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new account controller
        /// </summary>
        public AccountController()
        {
            authenticationRepository = new AuthenticationRepository();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Handles user registeration.
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ActionName("Register")]
        public async Task<IHttpActionResult> Register(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await authenticationRepository.RegisterUser(userModel);
            return Ok();
        }

        /// <summary>
        /// Handles user login.
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ActionName("Login")]
        public async Task<bool> Login(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return false;
            }

            var result = await authenticationRepository.FindUser(userModel.Username, userModel.Password);
            return (result != null);
        }

        /// <summary>
        /// Retrieves the latest client list.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ActionName("GetAllConnectedUsers")]
        public IEnumerable<string> GetAllConnectedUsers()
        {
            return ChatHub.Users.Select(x => x.Key);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Handles disposing of controller.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                authenticationRepository.Dispose();

            base.Dispose(disposing);
        }

        #endregion
    }
}