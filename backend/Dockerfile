# syntax=docker/dockerfile:1
#  FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
  FROM mcr.microsoft.com/dotnet/core/sdk:3.1

#  COPY bin/Release/netcoreapp3.1/ App/
  COPY build_output/ App/

  WORKDIR /App

  EXPOSE 80
  EXPOSE 443

  ENV ASPNETCORE_URLS=http://*:80
  ENV ASPNETCORE_ENVIRONMENT Prod

  ENTRYPOINT ["dotnet", "GS.Employee.API.dll"]
