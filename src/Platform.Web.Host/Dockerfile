# Build runtime image
FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY /out .
ENTRYPOINT ["dotnet", "Platform.Web.Host.dll"]
