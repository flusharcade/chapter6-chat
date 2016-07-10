// --------------------------------------------------------------------------------------------------
//  <copyright file="AuthenticationContext" company="Flush Arcade Pty Ltd.">
//    Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace Chat.Repositories
{
    using Microsoft.AspNet.Identity.EntityFramework;

    /// <summary>
    /// The authentication context
    /// </summary>
    public class AuthenticationContext : IdentityDbContext<IdentityUser>
    {
        /// <summary>
        /// Initializes a new authentication context
        /// </summary>
        public AuthenticationContext()
            : base("AuthenticationContext")
        {
        }
    }
}