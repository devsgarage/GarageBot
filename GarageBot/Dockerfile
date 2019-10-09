FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
COPY ["GarageBot/GarageBot.csproj", "GarageBot/"]
COPY ["Service.Twitch/Service.Twitch.csproj", "Service.Twitch/"]
COPY ["Service.Core/Service.Core.csproj", "Service.Core/"]
COPY ["ChatCommands/ChatCommands.csproj", "ChatCommands/"]
COPY ["Service.Infrastructure/Service.Infrastructure.csproj", "Service.Infrastructure/"]
COPY ["ChatServices/ChatServices.csproj", "ChatServices/"]
RUN dotnet restore "GarageBot/GarageBot.csproj"
COPY . .
WORKDIR "/src/GarageBot"
RUN dotnet build "GarageBot.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "GarageBot.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "GarageBot.dll"]