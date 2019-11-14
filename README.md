# MicroServico

### Summário

#### Azure

- Azure Database for PostgreSQL servers
- Azure Container registries
- Azure App Services 
   - Publish 
      - Docker Container

#### Docker build

- docker-compose up --build

#### Subindo manualmente para o Azure

Subindo as imagens para o Azure Container registries

- docker login microservicos.azurecr.io -u <USUÁRIO> -p <SENHA>
- docker tag <image_name> microservicos.azurecr.io/<image_name>
- docker pull microservicos.azurecr.io/<image_name>
   
Sobe o arquivo docker-compose_PROD.yml no Azure App Services
 
#### CI/CD Azure DevOps

Usando o CI/CD do Azure DevOps segue o procedimento.

### Tecnologias usada
 - .net core 2.2
 - postgresql
 - gateway ocelot
 - jwt token
 
 
