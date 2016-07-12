// --------------------------------------------------------------------------------------------------
//  <copyright file="UserModel" company="Flush Arcade Pty Ltd.">
//    Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace Chat.Models 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// The user model.
    /// </summary>
    public class UserModel
    {
        #region Public Properties

        /// <summary>
        /// The username.
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// The password.
        /// </summary>
        [Required]
        public string Password { get; set; }

        #endregion
    }
}