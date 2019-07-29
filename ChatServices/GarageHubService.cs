using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Service.Core;

namespace ChatServices
{
    public class GarageHubService : IHubService
    {
        public event EventHandler OnAlertReceived;
        public event EventHandler OnCelebrateReceived;
        HubConnection hubConnection;
        string hubUrl;

        public GarageHubService(string hubUrl)
        {
            this.hubUrl = hubUrl;
            //hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:44361/garageHub").Build();
            hubConnection = new HubConnectionBuilder().WithUrl(hubUrl).Build();
            hubConnection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await hubConnection.StartAsync();
            };
        }

        public async Task Connect()
        {
            if (hubConnection.State == HubConnectionState.Connected)
                return;

            hubConnection.On<string>("AlertReceived", (user) =>
            {
                OnAlertReceived?.Invoke(this, new EventArgs());
            });

            hubConnection.On<string>("Celebrate", (user) =>
            {
                OnCelebrateReceived?.Invoke(this, new EventArgs());
            });


            try
            {
                await hubConnection.StartAsync();
                Console.WriteLine("Connected to GarageHub");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error connecting to GarageHub");
                Console.WriteLine(ex);
            }
        }

        public async Task SendAlert(string user)
        {
            if (hubConnection.State == HubConnectionState.Disconnected)
            {
                throw new Exception("Connection must be started before sending an alert. Use the Connect method to connect to the Hub");
            }

            await hubConnection.SendAsync("AlertBroadcaster", user);
            await hubConnection.SendCoreAsync("AlertBroadcaster", new[] { user });
        }

        public async Task SendCelebration(string user)
        {
            if (hubConnection.State == HubConnectionState.Disconnected)
            {
                throw new Exception("Connection must be started before sending an alert. Use the Connect method to connect to the Hub");
            }

            await hubConnection.SendAsync("Celebrate", user);
        }

    }
}
