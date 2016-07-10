
namespace Chat.Models 
{
    using System.Collections.Generic;

    /// <summary>
    /// The SignalR user.
    /// </summary>
    public class SigRUser
    {
        #region Public Properties

        /// <summary>
        /// The name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The set of connection ids.
        /// </summary>
        public HashSet<string> ConnectionIds { get; set; }

        #endregion
    }
}