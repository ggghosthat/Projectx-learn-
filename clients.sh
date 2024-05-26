#!/bin/bash

dotnet_config=Release
projectx_path_client=./utilities/Projectx.Client/
projectx_path_watcher=./utilities/Projectx.Watcher/

printf "Preparing output directories for client build ...\n"
mkdir -p $projectx_path_client
mkdir -p $projectx_path_watcher
rm -rf $projectx_path_client + '*'
rm -rf $projectx_path_watcher + '*'

printf "Building clients for docker container ...\n"
dotnet clean ./Projectx.Client/ -c $dotnet_config > /dev/null
dotnet clean ./Projectx.Watcher/ -c $dotnet_config > /dev/null
dotnet build ./Projectx.Client -c $dotnet_config > /dev/null

printf "Publishing clients for docker container ...\n"
dotnet publish ./Projectx.Client/  -c $dotnet_config --framework net8.0 --runtime linux-x64 -o $projectx_path_client > /dev/null
dotnet publish ./Projectx.Watcher/ -c $dotnet_config --framework net8.0 --runtime linux-x64 -o $projectx_path_watcher > /dev/null

printf "Building docker container ...\n"
docker build --tag 'projectx-client' ./utilities
clear
printf "Client docker container ready to use.\n"
docker run -it --entrypoint bash --network=projectx_projectx-net projectx-client
