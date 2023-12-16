# DatabaseModule  
Serves as the interface for seamless interaction with MongoDB. Manages entities such as therapists, patients, applications, and modules. Controllers handle various data operations.  


## Setup    
*Database Setup*  
- docker run -d --name mongodb -p 27017:27017 -e MONGO_INITDB_ROOT_USERNAME=ROOT -e MONGO_INITDB_ROOT_PASSWORD=ROOT mongo  
- docker run --name my-redis -p 6379:6379 -d redis  

*API Setup*  
- docker build . -t databaseapi  
- docker run --name DatabaseApi -p 9010:443 -p 8010:80 -d databaseapi   




        