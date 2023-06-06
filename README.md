# DatabaseModule
Contains the Project Database and the API that performs CRUD operations

## Setup    
*Database Setup*  
- docker run -d --name mongodb -p 27017:27017 -e MONGO_INITDB_ROOT_USERNAME=ROOT -e MONGO_INITDB_ROOT_PASSWORD=ROOT mongo  

*API Setup*  
- docker build . -t databaseapi  
- docker run --name DatabaseApi -p 9030:443 -p 8090:80 -d databaseapi