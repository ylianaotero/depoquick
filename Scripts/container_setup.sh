#!/bin/bash

# Pull the Azure SQL Edge image from Microsoft's container registry
docker pull mcr.microsoft.com/azure-sql-edge

# Run the Docker container with the specified environment variables and port mapping
docker run -d --name sqlserver -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Passw1rd' -p 1433:1433 mcr.microsoft.com/azure-sql-edge