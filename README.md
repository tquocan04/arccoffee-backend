# Arc Coffee

Arc Coffee is a RESTful API backend for managing bottled water products and orders. Built with ASP.NET Core, it leverages Entity Framework Core, SQL Server, JWT authentication, and Cloudinary for image storage. The application is containerized using Docker and deployed with Nginx as a reverse proxy.

## Features
- **Product Management**: Create, read, update, and delete bottled water products.
- **Order Management**: Handle customer orders with status tracking.
- **Authentication**: Secure endpoints with JWT-based authentication.
- **Image Storage**: Upload and manage product images using Cloudinary.
- **Database**: SQL Server with Entity Framework Core for data persistence.
- **Deployment**: Containerized with Docker and orchestrated via Docker Compose, served through Nginx.


## Tech Stack
- **Backend**: ASP.NET Core Web API
- **Database**: SQL Server, Entity Framework Core
- **Authentication**: JWT
- **Image Storage**: Cloudinary
- **Web Server**: Nginx
- **Containerization**: Docker, Docker Compose


## Prerequisites
- .NET 8 SDK
- Docker and Docker Compose
- SQL Server (local or cloud-based)
- Cloudinary account with API credentials


## Setup Instructions

1. Clone the Repository:

```bash
git clone https://github.com/tquocan04/arccoffee-backend.git
cd arccoffee-backend
```

2. Configure Environment Variables:

- Copy .env.example to .env and update with your SQL Server connection string, JWT secret, and Cloudinary credentials.

```text
MSSQL_SA_PASSWORD=your_mssql_sa_password
DATABASE_HOST=your_database_host
DATABASE_NAME=your_database_name
DATABASE_USER=sa
DATABASE_PASSWORD=your_database_password
DATABASE_PORT=1433

DATABASE_CONNECTION="Server=${DATABASE_HOST};Database=${DATABASE_NAME};User Id=sa;Password=${DATABASE_PASSWORD};TrustServerCertificate=true"

# Environment and Ports
ASPNETCORE_ENV=Development
APP_PORT=8080
APP_INTERNAL_PORT=8080
SQLSERVER_PORT=1434

NGINX_HTTP_PORT=80

# Cloudinary Configuration
CLOUDINARY_CLOUD_NAME=
CLOUDINARY_API_KEY=
CLOUDINARY_API_SECRET=

# JWT
Jwt_Secret=your_jwt_secret_key
Jwt_Issuer=http://your_domain:8080
Jwt_Audience=https://your_domain:80
Jwt_TokenExpired=1
```

3. Run with Docker:
- Build and start the containers:
```bash
docker-compose up -d --build
```
- The API will be available at http://localhost:8080.
- Access Swagger UI for API testing at http://localhost:8080/swagger/index.html.


## Testing the API
- Use Swagger UI at http://localhost:8080/swagger/index.html to explore and test API endpoints interactively.
- Ensure the API is running (http://localhost:8080) before accessing Swagger.


## Running Locally (without Docker)
1. Ensure SQL Server is running and the connection string is updated in appsettings.json.
2. Run the application:

```bash
dotnet run
```


## Deployment
- Use the provided Dockerfile and docker-compose.yml for production deployment.
- Configure Nginx in nginx.conf for reverse proxy.
- Deploy to a cloud provider (e.g., AWS, Azure) or a VPS with Docker support.