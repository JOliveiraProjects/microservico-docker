# MicroServico

### Summário

#### Azure

* Azure Database for PostgreSQL servers
* Azure Container registries
* Azure App Services 
   * Publish 
      * Docker Container

#### Docker build

```sh

docker-compose up --build

```

#### Subindo manualmente para o Azure

Subindo as imagens para o **Azure Container registries**

```sh

docker login <AZURE_CONTAINER_REGISTRIES_NAME>.azurecr.io -u <USUARIO> -p <SENHA>
docker tag <IMAGE_NAME>:<TAG> <AZURE_CONTAINER_REGISTRIES_NAME>.azurecr.io/<IMAGE_NAME>
docker pull <AZURE_CONTAINER_REGISTRIES_NAME>.azurecr.io/<IMAGE_NAME>

```
   
Sobe o arquivo `docker-compose_PROD.yml` no **Azure App Services**
 
#### CI/CD Azure DevOps

Usando o CI/CD do **Azure DevOps** segue o procedimento.

### Tecnologias usada
 * .net core 2.2
 * postgresql
 * gateway ocelot
 * jwt token
