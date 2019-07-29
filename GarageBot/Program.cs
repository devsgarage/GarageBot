using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Service.Twitch;
using System;
using System.Net.Http;
using System.Text;
using Service.Core;
using ChatServices;
using Service.Infrastructure;

namespace GarageBot
{
    public class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {

            await new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddUserSecrets<Program>();
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging();
                    services.AddOptions();
                    services.AddCommands();
                    var twitchSettings = hostContext.Configuration.GetSection("twitch").Get<TwitchSettings>();
                    services.AddSingleton(twitchSettings);

                    services.AddHttpClient<TwitchService>();
                    services.AddHttpClient<Proxy>();
                    services.AddSingleton<TwitchChatClient>();
                    services.AddSingleton<IHostedService, Bot>();
                    var test = hostContext.Configuration["TestVar"];
                    var hubUrl = hostContext.Configuration["GarageBotHub"];
                    Console.WriteLine($"hub url: {hubUrl}");
                    services.AddSingleton<IHubService, GarageHubService>((provider)=> new GarageHubService(hubUrl));
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConsole();
                })
                .RunConsoleAsync();
        }
    }
}
