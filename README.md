# DatabaseModule
Contains the Project Database and the API that performs CRUD operations

## Setup    
*Database Setup*  
- docker run -d --name mongodb -p 27017:27017 -e MONGO_INITDB_ROOT_USERNAME=ROOT -e MONGO_INITDB_ROOT_PASSWORD=ROOT mongo  
- docker run --name my-redis -p 6379:6379 -d redis

*API Setup*  
- docker build . -t <image_name>  
- docker run --name <container_name> -p <https_port>:443 -p <http_port>:80 -d <image_name>




        