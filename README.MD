# FCGames

**Nome em Inglês:** Fiap Cloud Games  
**Nome em Português:** Fiap Jogos na nuvem

## Integrantes do Grupo 127

- Saulo Szmyhiel Ganança

## Descrição

Este é um projeto de pós-graduação em Arquitetura de Software, desenvolvido pelo Grupo 127 da FIAP. O **FCGames** tem como objetivo fornecer uma estrutura moderna e escalável para vendas de jogos e gestão de servidores para partidas online, aplicando os principais conceitos de arquitetura de software, boas práticas de desenvolvimento e padrões de mercado.

## Tecnologias Utilizadas

- .NET 8
- C# 12
- DDD (Domain-Driven Design)
- TDD (Test-Driven Development)
- BDD (Behavior-Driven Development)
- SQL Server (via Docker)
- Docker

## Conteúdo do Projeto

### **Fase 1**

- Novidades do .NET 8 e C# 12
- Criação de API REST com .NET
- Middlewares e Injeção de Dependência
- Implementação de Logs
- Serialização com JSON e MessagePack
- Autenticação e Autorização
- Swagger e documentação de API
- Implementação de Cache

## Estrutura do Projeto

A estrutura inicial segue o padrão **DDD (Domain-Driven Design)**, dividida em camadas:

```
src/
├── FCGames.API             → Camada de apresentação (API REST)
├── FCGames.Application     → Casos de uso e lógica de aplicação
├── FCGames.Domain          → Entidades de domínio e contratos
└── FCGames.Infrastructure  → Implementações de persistência e serviços externos
```

Essa organização promove separação de responsabilidades, facilitando testes, manutenibilidade e escalabilidade.

## Como rodar o projeto

### **Pré-requisitos**

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)

### **Banco de Dados com Docker**

Execute no PowerShell (modo administrador):

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=@fiap2025" -p 1433:1433 --name SqlServerFiap -d mcr.microsoft.com/mssql/server:2022-latest
```

### **Executar Migrations**

```bash
dotnet ef migrations add "NomeDaMigration" --project FCGames.Infrastructure --startup-project FCGames.API
dotnet ef database update --project FCGames.Infrastructure --startup-project FCGames.API
```

## Contribuição

Se deseja contribuir, fique à vontade para abrir issues ou enviar pull requests. Toda colaboração é bem-vinda!

## Licença

Este projeto está licenciado sob a **MIT License**.
