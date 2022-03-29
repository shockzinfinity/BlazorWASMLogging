#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BlazorWASMSerilog/Server/BlazorWASMSerilog.Server.csproj", "BlazorWASMSerilog/Server/"]
COPY ["BlazorWASMSerilog/Client/BlazorWASMSerilog.Client.csproj", "BlazorWASMSerilog/Client/"]
COPY ["BlazorWASMSerilog/Shared/BlazorWASMSerilog.Shared.csproj", "BlazorWASMSerilog/Shared/"]
RUN dotnet restore "BlazorWASMSerilog/Server/BlazorWASMSerilog.Server.csproj"
COPY . .
WORKDIR "/src/BlazorWASMSerilog/Server"
RUN dotnet build "BlazorWASMSerilog.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorWASMSerilog.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazorWASMSerilog.Server.dll"]
