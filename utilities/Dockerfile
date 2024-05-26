FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY  Projectx.Client/ ./Projectx.Client
COPY  Projectx.Watcher/ ./Projectx.Watcher
ENV PROJECTX_APP_PATH=http://projectx_app_1:5000
ENTRYPOINT [ "bash" ]
