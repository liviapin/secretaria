# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Secretaria.Api/*.csproj ./Secretaria.Api/
COPY Secretaria.Aplicacao/*.csproj ./Secretaria.Aplicacao/
COPY Secretaria.Dominio/*.csproj ./Secretaria.Dominio/
COPY Secretaria.DataTransfer/*.csproj ./Secretaria.DataTransfer/
COPY Secretaria.Infra/*.csproj ./Secretaria.Infra/
RUN dotnet restore Secretaria.Api

# copy everything else and build app
COPY . .
RUN dotnet publish Secretaria.Api -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
EXPOSE 8080
ENTRYPOINT ["dotnet", "Secretaria.Api.dll"]