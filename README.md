# SapMvcApp

Este é um projeto de API construído com ASP.NET Core, que interage com uma API SAP. O objetivo é demonstrar como fazer chamadas HTTP para uma API SAP, gerenciar sessões e tokens CSRF, e expor rotas para interagir com os dados SAP de forma simples.

## Funcionalidades

- **Interação com API SAP:** Realiza chamadas de `POST` para a API SAP utilizando autenticação e gerenciamento de CSRF.
- **Rota de Exemplo:** A API expõe rotas como `/sap/serv` para interação com a API SAP e `/sap/printnome` para testar a recepção de dados no corpo da requisição e cabeçalhos.
- **Autenticação Básica:** O projeto usa autenticação básica para acessar a API SAP.

## Estrutura do Projeto

A estrutura do projeto é a seguinte:
SapMvcApp/ ├── Controllers/ │ └── SapController.cs # Controlador que expõe as rotas para interagir com a API SAP ├── Models/ │ 
└── NomeRequest.cs # Modelo que representa a estrutura de um nome de requisição ├── Services/ │
└── SapService.cs # Serviço responsável pela lógica de interação com a API SAP
├── Program.cs # Configuração principal da aplicação └── README.md # Arquivo de documentação do projeto


### Controllers

- **SapController:** Contém as rotas da API:
  - **POST /sap/serv:** Realiza a chamada para o SAP utilizando um CSRF token.
  - **POST /sap/printnome:** Recebe um nome no corpo da requisição e um token no cabeçalho, retornando o nome recebido.

### Models

- **NomeRequest:** Modelo usado para deserializar a requisição contendo o nome.

### Services

- **SapService:** Contém a lógica de acesso e interação com a API SAP, incluindo a obtenção do CSRF token e a execução de chamadas `POST` para enviar dados.

## Pré-Requisitos

- **.NET 6 ou superior**: O projeto foi desenvolvido utilizando .NET 6.
- **ASP.NET Core**: Framework utilizado para criar a API.
- **SAP API**: Este projeto depende de uma API SAP configurada corretamente para funcionar.

## Configuração e Instalação

1. **Clone o repositório**:

   ```bash
   git clone https://github.com/seuusuario/SapMvcApp.git
   cd SapMvcApp


dotnet restore
