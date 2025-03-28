🎥 Movie Website API
A modern and scalable Web API built with ASP.NET Core, designed for managing movie-related data such as genres, actors, directors, and movies.
Built using Clean Architecture, JWT Authentication, and CQRS with MediatR.

🚀 Features

✅ JWT Authentication & Token Blacklist Middleware

✅ Clean Architecture (Domain, Application, Infrastructure, Persistence, API)

✅ CQRS with MediatR (Commands, Queries, Handlers, Validators)

✅ CRUD endpoints for Movies, Genres, Actors, Directors

✅ External Movie Integration with TMDB API

✅ FluentValidation for input validation

✅ Role Seeding with Default Admin

✅ Swagger with Bearer Token Authorization

✅ Rate Limiting Middleware

✅ HTTPS Enforcement

📁 Project Structure

API/ – ASP.NET Core Web API (Controllers, Middleware, Swagger)

Application/ – Application Layer

Features (CQRS: Commands, Queries, DTOs)

Services (e.g., ITokenService, IExternalMovieService)

Utilities (MessageGenerator)

Domain/ – Domain Models (Movie, Genre, Actor, Director, etc.)

Infrastructure/ – JWT, Identity, External API Services

Persistence/ – Entity Framework Core, DbContext, Repositories, Migrations

README.md – Project Documentation

⚙️ Getting Started

🔧 1. Clone the Repository

git clone https://github.com/JaFidAn/MovieWebsite.git
cd MovieWebsite

🛠️ 2. Configure appsettings.json

Update the following sections in API/appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Your SQL Server connection string"
},
"JwtSettings": {
  "Key": "YourSuperSecretKey",
  "Issuer": "MovieAuthAPI",
  "Audience": "MovieWebsiteUser",
  "DurationInMinutes": 60
},
"Tmdb": {
  "BaseUrl": "https://api.themoviedb.org/3",
  "ApiKey": "YourTMDBApiKey"
}

🛋️ 3. Apply Migrations

dotnet ef database update --project Persistence --startup-project API

▶️ 4. Run the Application

dotnet run --project API

🌐 5. Open Swagger UI

Navigate to:

https://localhost:5001/swagger

🔐 Authentication & Roles

JWT Bearer Authentication

Default Roles: Admin, User

Default Admin credentials:

Email: r.alagezov@gmail.com

Password: R@sim1984

Endpoints:

POST /api/auth/register

POST /api/auth/login

🔒 How to Use JWT in Swagger

Click the Authorize button in Swagger UI

Paste your token with the word Bearer:

Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

Click Authorize to test secured routes

🔍 External Movies Integration

Endpoint: GET /api/movies/external

Fetches popular movies from TMDB API

Supports pagination with ?PageNumber=1

📜 License
This project is open-source and free to use for personal or commercial purposes.

Licensed under the MIT License.

👨‍💼 Author
Created with ❤️ by Rasim Alagezov

📧 Email: r.alagezov@gmail.com

💻 GitHub: https://github.com/JaFidAn

📄 LinkedIn: https://www.linkedin.com/in/rasim-alagezov/

Feel free to reach out for collaboration or feedback!
