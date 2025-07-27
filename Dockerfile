# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY ["src/SeliseTaskManager.sln", "./"]

# Copy project files
COPY ["src/SeliseTaskManager.API/SeliseTaskManager.API.csproj", "SeliseTaskManager.API/"]
COPY ["src/SeliseTaskManager.Application/SeliseTaskManager.Application.csproj", "SeliseTaskManager.Application/"]
COPY ["src/SeliseTaskManager.Domain/SeliseTaskManager.Domain.csproj", "SeliseTaskManager.Domain/"]
COPY ["src/SeliseTaskManager.Infrastructure/SeliseTaskManager.Infrastructure.csproj", "SeliseTaskManager.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "SeliseTaskManager.API/SeliseTaskManager.API.csproj"

# Copy the rest of the code
COPY src/. ./

# Publish the API project
RUN dotnet publish "SeliseTaskManager.API/SeliseTaskManager.API.csproj" -c Release -o /app/publish

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy build output
COPY --from=build /app/publish .

# Expose port (update if using a different one)
EXPOSE 80

# ASP.NET will listen on port 80
ENV ASPNETCORE_URLS=http://+:80

# Run the app
ENTRYPOINT ["dotnet", "SeliseTaskManager.API.dll"]
