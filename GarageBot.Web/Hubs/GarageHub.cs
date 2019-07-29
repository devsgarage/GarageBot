using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace GarageBot.Web.Hubs
{
    public class GarageHub : Hub
    {
        public event EventHandler OnAlertReceived;
        public async Task AlertBroadcaster(string user)
        {
            OnAlertReceived?.Invoke(this, new EventArgs());
            await Clients.All.SendAsync("AlertReceived", user);
        }

        public async Task Celebrate(string user)
        {
            await Clients.All.SendAsync("Celebrate", user);
        }
    }
}
