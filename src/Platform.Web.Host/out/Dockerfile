FROM microsoft/dotnet:2.2-aspnetcore-runtime-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk-stretch AS build
WORKDIR /src
COPY ["src/Platform.Web.Host/Platform.Web.Host.csproj", "src/Platform.Web.Host/"]
COPY ["src/Platform.Web.Core/Platform.Web.Core.csproj", "src/Platform.Web.Core/"]
COPY ["src/Platform.EntityFrameworkCore/Platform.EntityFrameworkCore.csproj", "src/Platform.EntityFrameworkCore/"]
COPY ["src/Platform.Core/Platform.Core.csproj", "src/Platform.Core/"]
COPY ["src/Platform.Application/Platform.Application.csproj", "src/Platform.Application/"]
RUN dotnet restore "src/Platform.Web.Host/Platform.Web.Host.csproj"
COPY . .
WORKDIR "/src/src/Platform.Web.Host"
RUN dotnet build "Platform.Web.Host.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Platform.Web.Host.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Platform.Web.Host.dll"]