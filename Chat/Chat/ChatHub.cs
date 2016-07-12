// --------------------------------------------------------------------------------------------------
//  <copyright file="ChatHub" company="Flush Arcade Pty Ltd.">
//    Copyright (c) 2016 Flush Arcade Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace Chat
{
    using System;
    using System.Web;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Security.Claims;

    using Microsoft.AspNet.SignalR;

    using Newtonsoft.Json;

    using Chat.Models;

    /// <summary>
    /// The chat hub.
    /// </summary>
    [Authorize]
    public class ChatHub : Hub
    {
        #region Static Properties

        /// <summary>
        /// The list of connected users.
        /// </summary>
        public static readonly ConcurrentDictionary<string, SigRUser> Users
            = new ConcurrentDictionary<string, SigRUser>(StringComparer.InvariantCultureIgnoreCase);

        #endregion

        #region Public Methods

        /// <summary>
        /// Sends a message from client to another client.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="to"></param>
        public void Send(string message, string to)
        {
            SigRUser receiver;

            if (Users.TryGetValue(to, out receiver))
            {
                var userName = (Context.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(claim => claim.Type == "UserName").Value;

                SigRUser sender;
                Users.TryRemove(userName, out sender);

                lock (receiver.ConnectionIds)
                {
                    foreach (var cid in receiver.ConnectionIds)
                    {
                        Clients.Client(cid).displayMessage("chat", message);
                    }
                }
            }
        }

        /// <summary>
        /// Sends all other connected users the list of current clients connected.
        /// </summary>
        /// <param name="userName"></param>
        public void NotifyOtherConnectedUsers(string userName)
        {
            var connectionIds = Users.Where(x => !x.Key.Contains(userName))
            .SelectMany(x => x.Value.ConnectionIds)
            .Distinct();

            foreach (var cid in connectionIds)
            {
                Clients.Client(cid).displayMessage("clients", JsonConvert.SerializeObject(Users.Select(x => x.Key)));
            }
        }

        /// <summary>
        /// Notified when a user connects
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            var userName = (Context.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(claim => claim.Type == "UserName").Value;
            string connectionId = Context.ConnectionId;

            var user = Users.GetOrAdd(userName, _ => new SigRUser
            {
                Name = userName,
                ConnectionIds = new HashSet<string>()
            });

            lock (user.ConnectionIds)
            {

                user.ConnectionIds.Add(connectionId);
                NotifyOtherConnectedUsers(userName);
            }

            return base.OnConnected();
        }

        /// <summary>
        /// Notified when a user disconnects
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            var userName = (Context.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(claim => claim.Type == "UserName").Value;
            string connectionId = Context.ConnectionId;

            SigRUser user;
            Users.TryGetValue(userName, out user);

            if (user != null)
            {
                lock (user.ConnectionIds)
                {
                    SigRUser removedUser;
                    Users.TryRemove(userName, out removedUser);

                    NotifyOtherConnectedUsers(userName);
                }
            }

            return base.OnDisconnected(stopCalled);
        }

        #endregion
    }
}