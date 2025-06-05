# Use official .NET 10 SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["JwtAuthApi.csproj", "."]
RUN dotnet restore "./JwtAuthApi.csproj"
COPY . .
RUN dotnet publish "JwtAuthApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use official .NET 10 ASP.NET runtime image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "JwtAuthApi.dll"]
