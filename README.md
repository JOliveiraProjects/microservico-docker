# MicroServico

###Summário###

-- Docker build 

docker-compose up --build
docker login microservicos.azurecr.io -u <USUÁRIO> -p <SENHA>
docker tag <image_name> microservicos.azurecr.io/<image_name>
docker pull microservicos.azurecr.io/<image_name>
 
 
-- CI/CD Azure DevOps

###Tecnologias usada###
 - .net core 2.2
 - postgresql
 - gateway ocelot
 
 
