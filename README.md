# TEST SysLog!

Projeto criado para teste de desenvolvedor. Com a ideia de cadastrar e rastrear pedidos.

# Descrição

Projeto construído utilizando  [.net e angular](https://learn.microsoft.com/pt-br/aspnet/core/client-side/spa/angular?view=aspnetcore-8.0&tabs=visual-studio)

# Requisitos

 - .Net 8
 - Mongodb
 - NodeJs, [npm](https://www.npmjs.com/)


## Estrutura de projeto

 - TrackOrders (.net 8 host)
	 - ClientApp (SPA angular aplicação)
 - TrackOrders.Test (Testes de integração utilizando [containers](https://testcontainers.com/))

## Preparação para executar

Configure o arquivo `appsettings.json` conforme abaixo:

      "Settings": {
        "Jwt": {
          "Secret": "{{CHAVE SECRETA PARA GERAR JWT}}"
        }
      },
      "ConnectionStrings": {
        "Mongo": "{{ URL DO MONGO}}"
      },


## Execução

Para executar , pelo vscode apenas click F5 

O usuário **default** para a aplicação é

    email : admin@gmail.com 
    senha: 12345

O **swagger** da aplicação pode ser encontrado pela url:

https://localhost:7225/swagger

## Execução dos testes de integração *(docker necessário)

Execute o commando `dotnet test`no mesmo caminho onde se encontra o arquivo de solução do projeto **( *.sln)**
