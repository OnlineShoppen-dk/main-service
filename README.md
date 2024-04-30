# main-service


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
dotnet user-secrets set "Azurite:ConnectionString" "UseDevelopmentStorage=true;"
dotnet user-secrets set "Azurite:UseHttps" "false"
dotnet user-secrets set "Azurite:Container" "mainservicecontainer"
dotnet user-secrets set "ConnectionStrings:conn" "server=localhost;user=Root;password=Password;database=mainservicedb"

dotnet user-secrets set "KEY" "SZBNheG6DYChL2oyIo6Q3dAiK4sREZGPX6orWfH2Mk="
dotnet user-secrets set "ISSUER" "OnlineShoppen.dk"
dotnet user-secrets set "AUDIENCE" "OnlineShoppen.dk"
dotnet user-secrets set "CONNECTION_STRING" "server=localhost;user=Root;password=Password;database=mainservicedb"
```

### 
## Queues
- orderQueue : 

### Find Docker IP Address
```bash
docker inspect -f "{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}" main-service-db
```