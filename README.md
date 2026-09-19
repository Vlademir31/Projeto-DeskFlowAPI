# DeskFlow API

Sistema de gestão de chamados e helpdesk de TI.

---

## Indice

* [Sobre o Projeto](#sobre-o-projeto)
* [Objetivos](#objetivos)
* [Funcionalidades](#funcionalidades)
* [Ciclo de Vida do Chamado](#ciclo-de-vida-do-chamado)
* [Estrutura do Projeto](#estrutura-do-projeto)
* [Arquitetura](#arquitetura)
* [Tecnologias Utilizadas](#tecnologias-utilizadas)
* [Pré-requisitos](#prérequisitos)
* [Executando a Aplicação](#executando-a-aplicação)
* [Banco de Dados](#banco-de-dados)
* [Endpoints](#endpoints)
* [Tratamento de Exceções](#tratamento-de-exceções)
* [Git e Versionamento](#git-e-versionamento)
* [Video de Apresentação](#video-de-apresentação)
* [Licença](#licença)
* [Autor](#autor)

---

## Sobre o Projeto

O DeskFlow API é um projeto desenvolvido como projeto final avaliativo do módulo de Back End.

A aplicação consiste em uma Web API RESTful para gerenciamento de chamadas e helpdesk de TI, permitindo o cadastro de categorias, abertura e acompanhamento de chamados, registros de integrações e controle de ciclo de atendimento.

O projeto foi desenvolvido com foco na aplicação prática dos conceitos estudados durante o curso, utilizando C#, .NET10, ASP.NET Core API, Entity Framework Core e SQL Server.

---

## Objetivos

O projeto tem como objetivo aplicar de forma integrada os principais conceitos estudados no módulo de Back End, incluindo:

* C# e tipos de dados;
* Programação Orientada a Objetos (POO);
* Classes, propriedades e encapsulamento;
* ASP.NET Core Web API;
* APIs RESTful;
* Injeção de dependÊncia;
* Arquitetura em camadas;
* Serviços e Repositórios;
* Entity Framework Core;
* SQL Server;
* EF Core Migrations;
* Validações e regras de negócios;
* Tratamento global de exceções;
* Código de Status;
* Git e GitHub;
* Documentação técnica.

---

## Funcionalidades

As funcionalidades previstas para a DeskFlow API incluem:

* Cadastro de categoria;
* Consulta de categorias;
* Atualização de categorias;
* Exclusão de categorias;
* Abertura de chamados;
* Definição de prioridade;
* Controle do status do chamado;
* Inicio do atendimento;
* Encerramento do chamado;
* Registro de solução;
* Registro de interações;
* Consulta detalhada de chamados;
* Filtros combinados de chamados;
* Tratamento global de exceções.

As funcionalidades serão marcadas como implantadas conforme o desenvolvimento do projeto avançar.

---

## Ciclo de Vida do chamado

Um chamado poderá passar pelos seguintes estados:

### Aberto

chamado registrado e aguardando atendimento.

### Em Andamento

Chamado que está sendo atendido pela equipe de suporte.

### Fechado

Chamado encerrado com uma solução registrada e uma data de fechamento.

As regras de transição entre os estados serão implementados na camada **Services**.

---

# Estrutura do Projeto

```Text

DeskFlow/
│      
├──  docs/
│     
├──src/
│   │ 
│   └── DeskFlow.API/
│       │ 
│       ├── Controllers/
│       │ 
│       ├── Data/
│       │ 
│       ├── Middlewares/
│       │  
│       ├──Models/
│       │   │ 
│       │   └──Entities/
│       │       
│       ├──Repository/
│       │ 
│       └──Services/
│ 
└── tests

 ```
---

# Arquitetura

A API utiliza uma arquitetura organizada em camadas separando as responsabilidades da aplicação.
 ```Text
                       Cliente HTPP
                            │
                            ▼
                        Controller
                            │
                            ▼
                         Services
                            │
                            ▼
                        Repository    
                            │
                            ▼
                 Entitiy Framework Core
                            │
                            ▼
                        SQL Server
 ```

 ### Controllers

 Responsável por receber as requisições HTTP e chamar o Services e retornar as respostas apropriadas ao cliente.

 ### Services

 Responsável pelas regras de negócio, validações e controle do fluxo das operações.

 ### Repository

 Responsável pelo acesso aos dados e pela comunicação com o Entity Framework Core.

 ### Models / Entities

 Contêm as entidades utilizadas pelo sistema e que serão persistidas no banco de dados.

 ### Data

 Responsável pelo context do Entity Framework Core e pelas configurações relacionadas à persistência dos dados.

 ### Middlewares

 Responsável por comportamentos que atravessam a aplicação, incluindo o tratamento global de exceções.

---

# Tecnologias Utilizadas

* C#
* .NET10
* ASP.NET Core Web API
* Entity Framework Core 10
* SQL Server
* OpenAPI
* Git
* GitHub

---

### Pré/requisitos

Para executar o projeto será necessário ter instalado:

* .NET SDK 10;
* SQL Server ou uma instalação compativel;
* Git.

---

## Executando a aplicação

As instruções completas para configuração e execução serão atualizadas durante o desenvolvimento.

de forma geral a aplicação será executada utilizando:

dotnet restore
dotnet build
dotnet run

As instruções para configuração da conexão com o banco de dados e aplicação da migrations serão adicionadas nesta seção quando essa etapa for implementad.

---

# Banco de Dados

A aplicação utlizará **SQL Server** como banco de dados e **Entity Framework Core 10** como ORRM.

As alterações estruturais do banco de dados serão controlados por meio de  **EF Core Migrations**.

Os comandos necessários para  criação e atualização do banco serão documentados após a implementação do **DbContext** e da primeira Mgration.

---

# Endpoints

### Categorias

```Text
* Post / api / categorias
* Get / api / categorias
* Get / api / categorias / { id }
* Put / api / categorias / { id }
* Delet / api / categorias / { id }
```

### Chamados

```Text
* Post / api / chamados
* Get / api / chamados
* Get / api / chamados / { id }
* Put / api / chamados / { id } / iniciar
* Delet / api / chamados / { id } / encerrar
```
### Interações

* Post / api / chamados / { id } / interações

Os detalhes de parâmetros, respostas e códigos HTTP serão adicionados conforme os endpoints forem implementados e testados.

---

# Tratamento de Exceções

A aplicação utilizará um **Middleware** personalizado para realizar o tratamento global de exceções.

O objetivo é retornar respostas **HTTP** padronizadas e evitar que detalhes internos da aplicação como stack traces sejam expostos ao cliente.

Essa funcionalidade será implementada na camada de **Middlewares**.

# Git e Versionamento

O projeto utiliza **Git** para controle de versão e **GitHub** para hospedagem do repositório.

As alterações serão organizadas em commits pequenos e descritivos seguindo a convenção Convencional Commits sempre que aplicável.

## Video de Apresentação

O link para o video de apresentação será adicionado após a conclusão do projeto.

# Licença

Projeto desenvolvido para fins acadêmicos e de estudo.

# Autor

Vlademir Trindade de Oliveira
