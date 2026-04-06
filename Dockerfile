FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything from the repo
COPY . .

# Find the .Web project file specifically and restore it
RUN dotnet restore "MaxEndLabs.Web/MaxEndLabs.Web.csproj"

# Build and Publish the .Web project
WORKDIR "/src/MaxEndLabs.Web"
RUN dotnet publish "MaxEndLabs.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Important: ASP.NET 8 listens on 8080 by default
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# The DLL name usually matches the .csproj name
ENTRYPOINT ["dotnet", "MaxEndLabs.Web.dll"]
