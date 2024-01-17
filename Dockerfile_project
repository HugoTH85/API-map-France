# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS Build

WORKDIR /src

COPY . .

# Installer Git (si nécessaire)
RUN apt-get update \
    && apt-get install -y git \
    && rm -rf /var/lib/apt/lists/*

# Restaurer les dépendances et publier l'application
RUN dotnet restore "./carte-france-interactive.csproj" --disable-parallel \
    && dotnet publish "./carte-france-interactive.csproj" -c release -o /app --no-restore

# Stage de production
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

# Copier les fichiers publiés depuis l'étape de build
COPY --from=Build /app ./

# Exposer le port nécessaire pour l'application
EXPOSE 5000

# Commande d'entrée pour exécuter l'application
ENTRYPOINT [ "dotnet", "carte-france-interactive.dll" ]