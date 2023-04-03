#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

RUN apt update
RUN apt install -y curl
# SQL Server trusted connection problem
RUN sed -i 's/TLSv1.2/TLSv1.0/g' /etc/ssl/openssl.cnf

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["K8s.Training.Api/K8s.Training.Api.csproj", "K8s.Training.Api/"]
COPY ["K8s.Training.Data/K8s.Training.Data.csproj", "K8s.Training.Data/"]
COPY ["K8s.Training.Domain/K8s.Training.Domain.csproj", "K8s.Training.Domain/"]
RUN dotnet restore "K8s.Training.Api/K8s.Training.Api.csproj"
COPY . .
WORKDIR "/src/K8s.Training.Api"
RUN dotnet build "K8s.Training.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "K8s.Training.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "K8s.Training.Api.dll"]