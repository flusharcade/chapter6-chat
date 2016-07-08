
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

    [Authorize]
    public class ChatHub : Hub
    {
        public static readonly ConcurrentDictionary<string, SigRUser> Users
            = new ConcurrentDictionary<string, SigRUser>(StringComparer.InvariantCultureIgnoreCase);

        public void Send(string message)
        {
            string sender = Context.User.Identity.Name;

            Clients.All.received(new { sender = sender, message = message, isPrivate = false });
        }

        public void Send(string message, string to)
        {
            SigRUser receiver;

            if (Users.TryGetValue(to, out receiver))
            {
                var userName = (Context.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(claim => claim.Type == "UserName").Value;

                SigRUser sender = GetUser(userName);

                lock (receiver.ConnectionIds)
                {
                    foreach (var cid in receiver.ConnectionIds)
                    {
                        Clients.Client(cid).displayMessage("chat", message);
                    }
                }
            }
        }

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

        private SigRUser GetUser(string username)
        {
            SigRUser user;
            Users.TryGetValue(username, out user);

            return user;
        }
    }
}