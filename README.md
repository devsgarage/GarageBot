# GarageBot
.NET Chat bot designed to interact with and respond to users requests within the configured chat environment

## Supported Commands

List of commands available to users in chat

_note: all commands begin with a ```!```_
- ```list``` - display a list of available chat commands
- ```list <command>``` - display description of command
- ```alert``` - play an audible sound to get broadcasters attention
- ```celebrate``` - plays "celebration" music
    - _note: only available to broadcaster_
- ```idea``` - record a note to broadcaster or idea for stream improvement
- ```livecoders``` - display Twitch URL for the Live Coders team
- ```project``` - description of current project being worked
- ```repo``` - display url of current source code repository
- ```so <username>``` - give a shout out to a fellow streamer
- ```uptime``` - display the amount of time stream has been running
- ```test``` - test to confirm bot is functional
- ```testaudio``` - test audio functionality

Event based commands
- ```userjoined``` - repsonds to userjoined event and sends an automatic shout out to members of the Live Coders team

## Solution Breakdown

### ```ChatCommands.csproj``` 
Contains the commands available to the chat bot. All classes implement the ```IChatCommand``` interface and are dynamically loaded into the bot at runtime using reflection

### ```ChatServices.csproj```
Contains the different services supported by the bot. Currently, we support interacting with streaming services (```IChatService```)  and a SignalR hub service (```IHubService```) used for playing audio. 

### ```docker-compose.dcproj```
Contains the definition and dependencies of the chat bot environment.

### ```GarageBot.csproj```
Main entry point for the chat bot and definition for a hosting container

### ```GarageBot.Web.csproj```
Main entry point for the web project used to play audio files

### ```Service.Core.csproj```
Contains definitions of the models and interfaces shared across all projects. _**This project should never reference any other project**_

### ```Service.Infrastructure.csproj```
**//Obsolete - DO NOT USE//** - This project was originally used for interaction with the host system to play audio, but ended up going another route to improve portability across operation systems

### ```Service.Twitch```
Contains logic used to interact with the Twitch API's and Twitch Internet Relay Chat (IRC) interface.

## Technologies used in chat bot solution
- [.NET Core 3.0](https://dotnet.microsoft.com/download/dotnet-core/3.0)
- [Blazor Server](https://docs.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-3.0#blazor-server)
- [Docker](https://www.docker.com/products/developer-tools)

## How to run the bot

Run ```docker-compose -f "docker-compose.yml" -f "docker-compose.override.yml" up -d --build```