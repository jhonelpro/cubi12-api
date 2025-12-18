# Stage 1: Build environment with SDK
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /App

# Copy the entire content of the current directory to the container
COPY . ./

# Restore dependencies as distinct layers
RUN dotnet restore && dotnet publish -c Release -o out

# Stage 2: Runtime environment with only runtime dependencies
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /App

# Copy the published output from the build environment
COPY --from=build-env /App/out .

# Expose port (documenta que acepta cualquier puerto)
EXPOSE 80

# Install dotnet-ef tool so migrations can run
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Apply migrations and start the app
ENTRYPOINT ["bash", "-c", "dotnet ef database update && dotnet Cubitwelve.dll"]