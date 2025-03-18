# Base image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and restore dependencies
COPY ["ContainerSolutions.sln", "./"]
COPY ["WebApplication1/WebApplication1.csproj", "WebApplication1/"]
COPY ["WebApplication4/WebApplication4.csproj", "WebApplication4/"]
RUN dotnet restore

# Build and publish both applications
COPY . .
RUN dotnet publish "WebApplication1/WebApplication1.csproj" -c Release -o /app/WebApp1
RUN dotnet publish "WebApplication4/WebApplication4.csproj" -c Release -o /app/WebApp2

# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Use environment variables for different apps
COPY --from=build /app/WebApp1 ./WebApp1
COPY --from=build /app/WebApp2 ./WebApp2

# Expose ports
EXPOSE 8080 8081

# Environment variable to control app startup
ENV DOTNET_RUNNING_PROJECT="WebApplication1"

ENTRYPOINT ["sh", "-c", "dotnet /app/$DOTNET_RUNNING_PROJECT.dll"]
