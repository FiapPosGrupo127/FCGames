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

### **Dockerfile**

O projeto inclui um Dockerfile multi-stage otimizado para produção:

**Características do Dockerfile:**
- **Multi-stage build**: Reduz o tamanho da imagem final
- **Layer caching**: Copia .csproj primeiro para otimizar rebuild
- **Minimal runtime**: Usa `aspnet:8.0` em vez de `sdk:8.0` em produção
- **Security**: Não inclui ferramentas de desenvolvimento na imagem final

```dockerfile
# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia arquivos de projeto (.csproj) primeiro para cache de layers
COPY src/FCGames.API/*.csproj ./FCGames.API/
COPY src/FCGames.Application/*.csproj ./FCGames.Application/
COPY src/FCGames.Domain/*.csproj ./FCGames.Domain/
COPY src/FCGames.Infrastructure/*.csproj ./FCGames.Infrastructure/

# Restaura dependências (cached se .csproj não mudou)
RUN dotnet restore ./FCGames.API/FCGames.API.csproj

# Copia código fonte e faz publish
COPY src/ ./src/
WORKDIR /app/src/FCGames.API
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 7060
ENTRYPOINT ["dotnet", "FCGames.API.dll"]
```

**Benefícios do Multi-stage:**
- **Tamanho**: Imagem final ~263MB vs ~1.2GB com SDK
- **Segurança**: Sem ferramentas de build em produção
- **Performance**: Apenas runtime necessário

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

#### **Configuração do docker-compose.yml:**

```yaml
services:
  api:
    build: .
    container_name: API
    ports:
      - "7060:7060"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - DatabaseProvider=SqlServer
    depends_on:
      - sqlserver
    networks:
      - fcgames-network
      - monitoring

  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: SqlServerFiap
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=@fiap2025
    ports:
      - "1433:1433"
    networks:
      - fcgames-network

  prometheus:
    image: prom/prometheus:latest
    container_name: prometheus
    ports:
      - "9090:9090"
    volumes:
      - ./monitoring/prometheus.yml:/etc/prometheus/prometheus.yml
    networks:
      - monitoring

  grafana:
    image: grafana/grafana:latest
    container_name: grafana
    ports:
      - "3000:3000"
    environment:
      - GF_SECURITY_ADMIN_USER=admin
      - GF_SECURITY_ADMIN_PASSWORD=admin123
    networks:
      - monitoring
```

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

#### **Logs dos Containers:**
```bash
# Logs da API
docker logs API

# Logs em tempo real
docker logs -f API

# Logs do Prometheus
docker logs prometheus

# Logs do Grafana
docker logs grafana
```

#### **Gerenciamento dos Containers:**
```bash
# Parar todos os serviços
docker-compose down

# Parar e remover volumes
docker-compose down -v

# Rebuild dos containers
docker-compose up --build -d

# Ver status dos containers
docker-compose ps
```

### **Monitoramento e Observabilidade**

#### **Acessar Serviços:**

- **API**: http://localhost:7060
- **Swagger**: http://localhost:7060/swagger
- **Health Check**: http://localhost:7060/health
- **Métricas**: http://localhost:7060/metrics
- **Prometheus**: http://localhost:9090
- **Grafana**: http://localhost:3000

#### **Credenciais Grafana:**
- **Usuário**: `admin`
- **Senha**: `admin123`

#### **Configurar Data Source Prometheus no Grafana:**
1. Acesse Grafana: http://localhost:3000
2. Vá em Configuration → Data Sources
3. Adicione Prometheus com URL: `http://prometheus:9090`

### **Suporte Multi-Provider Database**

O projeto suporta múltiplos provedores de banco:

#### **SQL Server (Padrão):**
```yaml
environment:
  - DatabaseProvider=SqlServer
```

#### **PostgreSQL:**
```yaml
environment:
  - DatabaseProvider=PostgreSql
  - DATABASE_URL=postgresql://user:pass@host:5432/db
```

#### **InMemory (Testes):**
```yaml
environment:
  - DatabaseProvider=InMemory
```

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

### **Troubleshooting Docker**

#### **Problemas Comuns:**

**1. Porta já está em uso:**
```bash
# Verificar processos usando a porta
netstat -ano | findstr :7060

# Parar container específico
docker stop API
```

**2. Erro de conexão com banco:**
```bash
# Verificar se SQL Server está rodando
docker ps | grep SqlServerFiap

# Verificar logs do banco
docker logs SqlServerFiap
```

**3. Prometheus não conecta com a API:**
```bash
# Verificar rede Docker
docker network ls
docker network inspect fcgames_monitoring

# Verificar configuração do Prometheus
docker exec prometheus cat /etc/prometheus/prometheus.yml
```

**4. Grafana não conecta com Prometheus:**
- Use URL: `http://prometheus:9090` (não localhost)
- Verifique se ambos estão na mesma rede Docker

**5. Limpar ambiente Docker:**
```bash
# Remover containers parados
docker container prune

# Remover imagens não utilizadas
docker image prune

# Limpar tudo (cuidado!)
docker system prune -a
```

#### **Verificações de Saúde:**

```bash
# Status de todos os serviços
docker-compose ps

# Health check da API
curl http://localhost:7060/health

# Verificar métricas
curl http://localhost:7060/metrics

# Testar conectividade Prometheus
curl http://localhost:9090/api/v1/targets
```

## Contribuição

Se deseja contribuir, fique à vontade para abrir issues ou enviar pull requests. Toda colaboração é bem-vinda!

## Licença

Este projeto está licenciado sob a **MIT License**.
