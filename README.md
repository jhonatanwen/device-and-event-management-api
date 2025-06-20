# API de Gestão de Dispositivos e Eventos

## Como testar

1. Faça o download do arquivo de coleção de testes de requisição: [desafio-tecnico-nevoa-collection-jhonatan](./desafio-tecnico-nevoa-collection-jhonatan.json)
2. Importe ele no [Insomnia](https://insomnia.rest)**(Recomendado)** ou [Postman](https://www.postman.com)
3. [Inicie a API](#como-rodar-o-projeto)
4. Faça a [autenticação](#autenticação)
5. Use seu token no hearder para testar as rotas no seu aplicativo de escolha

## Como Rodar o Projeto

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop/) (para SQL Server)

### Setup Rápido

1. **Clone e restaure dependências**

```bash
git clone https://github.com/jhonatanwen/desafio-tecnico-nevoa.git
cd desafio-tecnico-nevoa
dotnet restore
```

2. **Configure o banco de dados com Docker**

Execute um dos scripts abaixo de acordo com seu sistema operacional para configurar automaticamente o banco de dados no docker

```bash
# Linux/Mac
setup-docker-sqlserver.sh

# Windows
setup-docker-sqlserver.bat
```

3. **Execute as migrations**

```bash
cd src/DeviceManagement.API
dotnet ef database update --project ../DeviceManagement.Infrastructure
```

4. **Execute a aplicação**

```bash
dotnet run
```

5. **Acesse a API**

- Endpoint base para diz que a API está funcionando: http://localhost:5231/api/v1
- Swagger: http://localhost:5231/swagger

### Configuração Manual do Banco

Se preferir usar SQL Server local ao invés do Docker, edite a connection string em `src/DeviceManagement.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=DeviceManagement;Integrated Security=true;TrustServerCertificate=true;"
  }
}
```

## Autenticação

Para testar endpoints você precisa se autenticar e pegar o token JWT:

1. **Login**

```js
POST /api/v1/auth/login
Content-Type: application/json

{
    "username": "admin",
    "password": "123456"
}
```

2. **Use o token retornado no header**

```http
Authorization: Bearer <token-jwt>
```

## Endpoints Principais

### Clientes

- `GET /api/v1/clientes` - Listar todos
- `GET /api/v1/clientes/{id}` - Buscar por ID
- `POST /api/v1/clientes` - Criar novo
- `PUT /api/v1/clientes/{id}` - Atualizar
- `DELETE /api/v1/clientes/{id}` - Excluir

### Dispositivos

- `GET /api/v1/dispositivos/{id}` - Buscar por ID
- `GET /api/v1/dispositivos/cliente/{clienteId}` - Buscar por cliente
- `POST /api/v1/dispositivos` - Criar novo

### Eventos

- `GET /api/v1/eventos/all` - Listar todos
- `GET /api/v1/eventos?startDate=2024-01-01&endDate=2024-12-31` - Por período
- `GET /api/v1/eventos/dispositivo/{id}` - Por dispositivo
- `POST /api/v1/eventos` - Registrar evento

### Dashboard

- `GET /api/v1/dashboard` - Estatísticas dos últimos 7 dias

## Arquitetura

```
src/
├── DeviceManagement.Domain/          # Entidades, Value Objects
├── DeviceManagement.Application/     # Casos de Uso, DTOs
├── DeviceManagement.Infrastructure/  # Repositórios, Migrations
└── DeviceManagement.API/            # Controllers, Validações
```

## Tecnologias

- .NET 8, Entity Framework Core 9
- SQL Server, JWT Authentication
- FluentValidation, Swagger/OpenAPI

### Obrigado pela oportunidade! 😄
