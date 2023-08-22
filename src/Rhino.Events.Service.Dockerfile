# Define the base image and working directory for the first stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

# Expose ports for the application
EXPOSE 9999

# Define the base image and working directory for the build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["Rhino.Events.Service/Rhino.Events.Service.csproj", "Rhino.Events.Service/"]
RUN dotnet restore "Rhino.Events.Service/Rhino.Events.Service.csproj"

# Copy the source code and build the application
COPY . .
WORKDIR "/src/Rhino.Events.Service"
RUN dotnet build "Rhino.Events.Service.csproj" -c Release -o /app/build

# Define the base image and working directory for the publish stage
FROM build AS publish
RUN dotnet publish "Rhino.Events.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Define the final image and working directory
FROM base AS final
WORKDIR /app

# Copy the published application from the build stage
COPY --from=publish /app/publish .

# Define the entry point command for running the application
ENTRYPOINT ["dotnet", "Rhino.Events.Service.dll"]