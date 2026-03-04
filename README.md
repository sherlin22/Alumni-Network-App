# Alumni Network API

Production-style ASP.NET Core Web API using Clean Architecture for a university alumni platform.

## Tech Stack
- ASP.NET Core Web API (`net9.0` in this environment; code style compatible with .NET 8)
- Clean Architecture
- EF Core + SQL Server
- JWT auth + role-based authorization
- Serilog structured logging
- Swagger/OpenAPI
- Global exception middleware
- Docker

## Solution Structure

```text
AlumniNetwork.sln
AlumniNetwork.API/
  Controllers/
  Middleware/
  Filters/
  DependencyInjection/
  Program.cs
AlumniNetwork.Application/
  Common/
  DTOs/
  Interfaces/
  Services/
  Validators/
  UseCases/
AlumniNetwork.Domain/
  Entities/
  Enums/
  ValueObjects/
AlumniNetwork.Infrastructure/
  DbContext/
  Repositories/
  Configurations/
  JWTServices/
  DependencyInjection/
  Migrations/
Dockerfile
```

## Database Model
- `Users`
- `Posts`
- `Comments`
- `Likes`

Relationships:
- User -> Posts (1:N)
- Post -> Comments (1:N)
- Post -> Likes (1:N)

## Key Endpoints

### Auth
- `POST /api/auth/register`
- `POST /api/auth/login`

### User
- `GET /api/users/profile`
- `PUT /api/users/profile`

### Posts
- `POST /api/posts`
- `GET /api/posts?pageNumber=1&pageSize=10`
- `POST /api/posts/{postId}/like`
- `POST /api/posts/{postId}/comment`
- `GET /api/posts/{postId}/comments`

### Alumni Search
- `GET /api/alumni/search?department=IT&year=2022&location=Chennai`

### Admin
- `GET /api/admin/users`
- `DELETE /api/admin/users/{id}`
- `DELETE /api/admin/posts/{id}`

## Standard Response

```json
{
  "success": true,
  "message": "Success",
  "data": {}
}
```

Paged response shape:

```json
{
  "success": true,
  "message": "Success",
  "data": {
    "totalCount": 120,
    "pageNumber": 1,
    "pageSize": 10,
    "data": []
  }
}
```

## Migration Commands

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate --project AlumniNetwork.Infrastructure --startup-project AlumniNetwork.API
dotnet ef database update --project AlumniNetwork.Infrastructure --startup-project AlumniNetwork.API
```

## Run Instructions

1. Update `AlumniNetwork.API/appsettings.json` connection string and JWT secret.
2. Restore/build:

```bash
dotnet restore AlumniNetwork.sln
dotnet build AlumniNetwork.sln
```

3. Run API:

```bash
dotnet run --project AlumniNetwork.API
```

4. Open Swagger:
- `https://localhost:7xxx/swagger`

## Docker

```bash
docker build -t alumni-network-api .
docker run -p 8080:8080 alumni-network-api
```
