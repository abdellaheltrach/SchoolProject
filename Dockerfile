# ---- Build Stage ----
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# Copy solution and project files first for better layer caching
COPY SchoolProject.slnx .
COPY School.Api/School.Api.csproj School.Api/
COPY School.Core/School.Core.csproj School.Core/
COPY School.Domain/School.Domain.csproj School.Domain/
COPY School.Infrastructure/School.Infrastructure.csproj School.Infrastructure/
COPY School.Service/School.Service.csproj School.Service/

# Restore dependencies
RUN dotnet restore School.Api/School.Api.csproj

# Copy remaining source code
COPY . .

# Build and publish
RUN dotnet publish School.Api/School.Api.csproj -c Release -o /app/publish --no-restore

# ---- Runtime Stage ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS runtime
WORKDIR /app

EXPOSE 8080

ENV ASPNETCORE_ENVIRONMENT=Development

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "School.Api.dll"]
