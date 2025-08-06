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

## 🐳 **Docker e Containerização**

### **🚀 Início Rápido com Docker**

```bash
# 1. Clone o repositório
git clone https://github.com/FiapPosGrupo127/FCGames.git
cd FCGames

# 2. Execute toda a stack
docker-compose up -d

# 3. Acesse a aplicação
# API: http://localhost:7060
# Swagger: http://localhost:7060/swagger
# Grafana: http://localhost:3000 (admin/admin123)
# Prometheus: http://localhost:9090
```

### **Docker Compose**

#### **Executar Stack Completa**

```bash
docker-compose up -d
```

#### **Serviços Incluídos:**

- **API**: Aplicação FCGames na porta `7060`
- **SQL Server**: Banco de dados na porta `1433`
- **Prometheus**: Métricas na porta `9090`
- **Grafana**: Dashboards na porta `3000`

### **Comandos Docker Úteis**

#### **Build da Imagem:**

```bash
# Build local
docker build -t fcgames-api .

# Build com tag para Docker Hub
docker build -t usuario/fcgames-api:latest .
```

#### **Executar Apenas a API:**

```bash
docker run -p 7060:7060 fcgames-api
```

#### **Executar com Variáveis de Ambiente:**

```bash
docker run -p 7060:7060 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e DatabaseProvider=PostgreSql \
  -e DATABASE_URL="sua-connection-string" \
  fcgames-api
```

### **Monitoramento e Observabilidade**

#### **Acessar Serviços:**

- **API**: http://localhost:7060
- **Swagger**: http://localhost:7060/swagger
- **Health Check**: http://localhost:7060/health
- **Métricas**: http://localhost:7060/metrics
- **Prometheus**: http://localhost:9090
- **Grafana**: http://localhost:3000

#### **Deploy em Produção**

#### **Imagem Pública no Docker Hub:**

A aplicação está disponível publicamente no Docker Hub:

```bash
# Pull da imagem
docker pull dinosauloo/fcgames-api:latest

# Executar diretamente
docker run -p 7060:7060 dinosauloo/fcgames-api:latest
```

**Link da imagem:** https://hub.docker.com/r/dinosauloo/fcgames-api

#### **Usando Docker Hub:**

```bash
# Push para Docker Hub
docker tag fcgames-api usuario/fcgames-api:v1.0.0
docker push usuario/fcgames-api:v1.0.0

# Deploy em servidor
docker pull usuario/fcgames-api:v1.0.0
docker run -d -p 7060:7060 usuario/fcgames-api:v1.0.0
```

#### **Usando Docker Compose em Produção:**

```bash
# Definir ambiente de produção
export ASPNETCORE_ENVIRONMENT=Production
export DATABASE_URL="sua-connection-string-producao"

# Deploy
docker-compose -f docker-compose.yml up -d
```

## Contribuição

Se deseja contribuir, fique à vontade para abrir issues ou enviar pull requests. Toda colaboração é bem-vinda!

## Licença

Este projeto está licenciado sob a **MIT License**.
