# Script.ps1

# Change to the directory containing the docker-compose file
cd .\DatabaseApi\

# Run docker-build
docker build . -t databaseapi

# Run docker run container
docker run --name DatabaseApi -p 9010:443 -p 8010:80 -d databaseapi  
