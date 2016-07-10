// --------------------------------------------------------------------------------------------------
//  <copyright file="WebApiConfig" company="Flush Arcade Pty Ltd.">
//    Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace Chat
{
    using System.Web.Http;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The web API config.
    /// </summary>
    public static class WebApiConfig
    {
        #region Methods

        /// <summary>
        /// Configures the API controller routes.
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        #endregion
    }
}
