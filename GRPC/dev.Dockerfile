#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["GRPC/GRPC.Client.csproj", "GRPC/"]
RUN dotnet restore "GRPC/GRPC.Client.csproj"
COPY . .
WORKDIR "/src/GRPC"
RUN dotnet build "GRPC.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GRPC.Client.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY "GRPC/ssl/local.pfx" /app/ssl/local.pfx
ENTRYPOINT ["dotnet", "GRPC.Client.dll"]