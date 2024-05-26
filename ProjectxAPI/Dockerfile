FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY bin/Release/net8.0/linux-x64/publish .
ENV ASPNETCORE_URLS http://+:5000
EXPOSE 5000
ENTRYPOINT [ "dotnet", "ProjectxAPI.dll" ]
