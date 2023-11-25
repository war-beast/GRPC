#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SimpleGrpcService/SimpleGrpcService.csproj", "SimpleGrpcService/"]
RUN dotnet restore "SimpleGrpcService/SimpleGrpcService.csproj"
COPY . .
WORKDIR "/src/SimpleGrpcService"
RUN dotnet build "SimpleGrpcService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SimpleGrpcService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./Ssl/ /app/ssl
RUN chmod 600 /app/ssl/localhost.pfx -p pass
ENTRYPOINT ["dotnet", "SimpleGrpcService.dll"]