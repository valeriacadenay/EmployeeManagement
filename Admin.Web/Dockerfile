FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Admin.Web/Admin.Web.csproj", "Admin.Web/"]
COPY ["Management.Application/Management.Application.csproj", "Management.Application/"]
COPY ["Management.Domain/Management.Domain.csproj", "Management.Domain/"]
COPY ["Management.Infrastructure/Management.Infrastructure.csproj", "Management.Infrastructure/"]

RUN dotnet restore "Admin.Web/Admin.Web.csproj"

COPY . .

WORKDIR "/src/Admin.Web"
RUN dotnet publish "Admin.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Admin.Web.dll"]
