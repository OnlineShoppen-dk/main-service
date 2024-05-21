# main-service

## TODO:
- Optimize the CRUD operations, make them able to give proper response when error occurs, such as
  - When a product is not found
  - When a product is not deleted
  - When a product is not updated
  - etc...

## Submodule Testing
## Requirements
1. MySQL (This will be changed later when the project is deployed to Cloud)
2. Azurite (This will be changed later when the project is deployed to Cloud)
### Setup Azurite
1. Install Azurite
```bash
npm install -g azurite
```
## Setup User Secrets
Copy and Paste the following into the terminal to setup the user secrets for the project
```bash
dotnet user-secrets set "Jwt:Key" "SZBNheG6DYChL2oyIo6Q3dAiK4sREZGPX6orWfH2Mk="
dotnet user-secrets set "Jwt:Audience" "OnlineShoppen.dk"
dotnet user-secrets set "Jwt:Issuer" "OnlineShoppen.dk"
dotnet user-secrets set "Azurite:ConnectionString" "UseDevelopmentStorage=true;DevelopmentStorageProxyUri=http://host.docker.internal"
dotnet user-secrets set "Azurite:UseHttps" "false"
dotnet user-secrets set "Azurite:Container" "mainservicecontainer"
dotnet user-secrets set "ConnectionStrings:conn" "server=localhost;user=user;password=userpass;database=mainservicedb"
dotnet user-secrets set "RabbitMQ:Host" "localhost"
dotnet user-secrets set "RabbitMQ:User" "user"
dotnet user-secrets set "RabbitMQ:Pass" "userpass"
dotnet user-secrets set "RabbitMQ:ProductQueue" "productQueue"
dotnet user-secrets set "RabbitMQ:ProductSyncQueue" "productSyncQueue"
dotnet user-secrets set "RabbitMQ:ProductRemoveQueue" "ProductRemoveQueue"
```

### 
## Queues
- orderQueue : 

### Find Docker IP Address
```bash
docker inspect -f "{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}" main-service-db
```

### Alternative Tables
- DeletedProducts
- 