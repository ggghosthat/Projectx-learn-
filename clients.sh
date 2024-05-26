#!/bin/bash

dotnet_config=Release
projectx_path_client=./utilities/Projectx.Client/
projectx_path_watcher=./utilities/Projectx.Watcher/

mkdir -p $projectx_path_client
mkdir -p $projectx_path_watcher

rm -rf $projectx_path_client + '*'
rm -rf $projectx_path_watcher + '*'

dotnet clean ./Projectx.Client/ -c $dotnet_config
dotnet clean ./Projectx.Watcher/ -c $dotnet_config
dotnet build ./Projectx.Client -c $dotnet_config
dotnet publish ./Projectx.Client/  -c $dotnet_config --framework net8.0 --runtime linux-x64 -o $projectx_path_client
dotnet publish ./Projectx.Watcher/ -c $dotnet_config --framework net8.0 --runtime linux-x64 -o $projectx_path_watcher

docker build --tag 'projectx-client' ./utilities 

echo "ProjectX clients are ready to launch. Please use the following command\n 'docker run -it --entrypoint bash --network=<CONTAINERS_NETWORK_NAME> <CONTAINER_NAME>' "
