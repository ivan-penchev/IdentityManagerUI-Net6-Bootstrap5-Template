#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AspNetCore6WebApp1/AspNetCore6WebApp1.csproj", "AspNetCore6WebApp1/"]
COPY ["IdentityManagerUI/IdentityManagerUI.csproj", "IdentityManagerUI/"]
RUN dotnet restore "AspNetCore6WebApp1/AspNetCore6WebApp1.csproj"
COPY . .
WORKDIR "/src/AspNetCore6WebApp1"
RUN dotnet build "AspNetCore6WebApp1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AspNetCore6WebApp1.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AspNetCore6WebApp1.dll"]