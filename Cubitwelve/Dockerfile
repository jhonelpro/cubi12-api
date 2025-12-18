# Stage 1: Build environment with SDK
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /App

# Copia todo y restaura dependencias
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Stage 2: Runtime environment con solo runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /App

# Copia el resultado publicado
COPY --from=build-env /App/out ./

# Expone el puerto de la app
EXPOSE 80

# Solo corre la app, sin EF
ENTRYPOINT ["dotnet", "Cubitwelve.dll"]
