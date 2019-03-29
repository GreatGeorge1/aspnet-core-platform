# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2.3
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY /out .
ENTRYPOINT ["dotnet", "Platform.Web.Host.dll"]
