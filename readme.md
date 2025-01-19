# TodoX

A RESTful sample API built with .NET 9 for managing Todo items. 

## Prerequisites

- .NET 9 SDK
- PostgreSQL

## Getting Started

### 1. Clone the Repository

```bash
git clone git@github.com:Ivanshamir/todoX.git todo-api
cd todo-api
```
### 2. Set Up Environment Variables
Create a .env file in the solution root directory:
```
POSTGRES_CONNECTION="Host=localhost;Port=54328;Database=tododb;Username=root;Password=mypassword"
```
### 3. Install Dependencies
```
dotnet restore
```
### 4. Database Setup
```
# Install EF Core tools (if not already installed)
dotnet tool install --global dotnet-ef

# Apply database migrations
dotnet ef database update --project TodoApp.Infrastructure --startup-project TodoApp.Api
```
### 5. Run the Application 
```
cd src/TodoApp.Api
dotnet run
```

The API will be available at:
- HTTP: http://localhost:5107                   
- HTTPS: https://localhost:7157                 
- Swagger: http://localhost:5107/swagger/index.html

### API Endpoints
                                    
| Method | Endpoint                          | Description                  |
|--------|-----------------------------------|------------------------------|
| GET    | /api/todos                        | Get all todos                |
| GET    | /api/todos/{id}                   | Get a specific todo          |
| POST   | /api/todos                        | Create a new todo            |
| PUT    | /api/todos/{id}                   | Update a todo                |
| DELETE | /api/todos/{id}                   | Delete a todo                |
| PATCH  | /api/todos/{id}/complete          | Mark todo as completed       |
| GET    | /api/todos/completed              | Get all completed todos      |
| GET    | /api/todos/pending                | Get all pending todos        |
| DELETE | /api/todos/completed              | Delete all completed todos   |
