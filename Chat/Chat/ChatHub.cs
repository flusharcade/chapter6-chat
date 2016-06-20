using System;
using System.Web;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace Chat
{
    public static class UserHandler
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
    }

    public class ChatHub : Hub
    {
        public override Task OnConnected()
        {
            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            SendConnected();
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            SendConnected();
            return base.OnDisconnected(stopCalled);
        }

        public void SendChat(string connectionId, string message)
        {
            var client = Clients.Client(connectionId);

            if (client != null)
            {
                // Call the broadcastMessage method to update clients.
                Clients.Client(connectionId).displayMessage("chat", message);
            }
        }

        public void SendConnected()
        {
            var json = JsonConvert.SerializeObject(UserHandler.ConnectedIds);
            Clients.All.displayMessage("clients", json);
        }
    }
}