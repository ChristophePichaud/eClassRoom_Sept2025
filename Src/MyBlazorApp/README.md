# MyBlazorApp

## Description
MyBlazorApp is a web application built using Blazor WebAssembly and ASP.NET Core. It leverages Entity Framework Core with PostgreSQL for data management and provides a clean separation of concerns through a service layer and RESTful API endpoints.

## Project Structure
The project is organized into three main directories: `Client`, `Server`, and `Shared`.

- **Client**: Contains the Blazor WebAssembly application.
  - **Pages**: Blazor components for the application's pages.
  - **Shared**: Shared components and layouts used across different pages.
  - **Program.cs**: Entry point for the Blazor WebAssembly client application.

- **Server**: Contains the ASP.NET Core server application.
  - **Controllers**: API controllers that handle HTTP requests and responses.
  - **Data**: Database context and migration files.
    - **ApplicationDbContext.cs**: Defines the Entity Framework Core database context.
    - **Migrations**: Contains migration files for managing database schema changes.
  - **Models**: Data models representing the entities in the database.
  - **Services**: Service classes that encapsulate business logic and data access.
  - **Program.cs**: Entry point for the server application.
  - **Startup.cs**: Configures services and the application's request pipeline.

- **Shared**: Contains models that are used by both the client and server applications.

## Setup Instructions
1. Clone the repository to your local machine.
2. Navigate to the project directory.
3. Restore the NuGet packages by running:
   ```
   dotnet restore
   ```
4. Update the database connection string in `src/Server/appsettings.json` to point to your PostgreSQL database.
5. Run the migrations to set up the database:
   ```
   dotnet ef database update --project src/Server
   ```
6. Start the application by running:
   ```
   dotnet run --project src/Server
   ```

## Usage Guidelines
- Access the client application at `https://localhost:5001`.
- Use the API endpoints defined in the `Controllers` directory for data operations.
- Refer to the `Services` directory for business logic implementation.

## Contributing
Contributions are welcome! Please submit a pull request or open an issue for any enhancements or bug fixes.