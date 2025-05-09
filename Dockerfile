# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Accept version as build argument
ARG VERSION=1.0.0

# Copy csproj and restore as distinct layers
COPY ["src/Drive.Api/Drive.Api.csproj", "src/Drive.Api/"]
RUN dotnet restore "src/Drive.Api/Drive.Api.csproj"

# Copy everything else and build
COPY . .
WORKDIR /src/src/Drive.Api
RUN dotnet build "Drive.Api.csproj" -c Release -o /app/build -p:Version=$VERSION

# Stage 2: Publish the application
FROM build AS publish
RUN dotnet publish "Drive.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false -p:Version=$VERSION

# Stage 3: Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Add version label
ARG VERSION=1.0.0
LABEL version=$VERSION

# Expose the port your application listens on
EXPOSE 8080
EXPOSE 8081

# Set the entry point for the application
ENTRYPOINT ["dotnet", "Drive.Api.dll"]