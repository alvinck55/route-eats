# RouteEats

Route-aware restaurant discovery app. Enter an origin and destination, pick food categories, and get suggestions plotted along your driving route.

## Stack

| Layer | Tech |
|---|---|
| Frontend | React + Vite |
| Map | Google Maps JS API (`@react-google-maps/api`) |
| Backend | ASP.NET Core 8 Web API (C#) |
| Auth | ASP.NET Core Identity + JWT |
| ORM | Entity Framework Core |
| Database | PostgreSQL |

## Project Structure

```
route-eats/
├── backend/RouteEats.Api/      # ASP.NET Core Web API
│   ├── Controllers/            # RouteController, AuthController, etc.
│   ├── Services/               # DirectionsService, PolylineService, PlacesService
│   ├── Models/DTOs/            # Request/response records
│   ├── Models/Entities/        # EF Core entities (AppUser, SavedRoute)
│   ├── Data/                   # AppDbContext
│   └── Program.cs
├── frontend/                   # React + Vite SPA
│   └── src/
│       ├── components/         # SearchForm, MapView, RestaurantCard, CategoryFilter
│       ├── hooks/              # useCategories
│       └── services/           # api.js (Axios client)
└── docker-compose.yml          # PostgreSQL
```

## Getting Started

### 1. Start the database

```bash
docker compose up -d
```

### 2. Configure secrets

Copy `backend/RouteEats.Api/appsettings.json` and add your keys:

```json
{
  "GoogleApis": { "ApiKey": "<your-google-api-key>" },
  "Jwt": { "Key": "<min-32-char-secret>" }
}
```

> Use `dotnet user-secrets` in development instead of editing appsettings.json directly.

### 3. Run the backend

```bash
cd backend/RouteEats.Api
dotnet restore
dotnet ef database update   # applies migrations
dotnet run
# API available at http://localhost:5000
```

### 4. Run the frontend

```bash
cd frontend
cp .env.example .env        # add VITE_GOOGLE_MAPS_API_KEY
npm install
npm run dev
# App available at http://localhost:5173
```

## API Endpoints

| Method | Path | Auth | Description |
|---|---|---|---|
| POST | `/api/route/suggestions` | — | Get restaurants along a route |
| GET | `/api/categories` | — | List food categories |
| POST | `/api/auth/register` | — | Register a new user |
| POST | `/api/auth/login` | — | Get JWT token |
| GET | `/api/user/saved-routes` | JWT | List saved routes |
| POST | `/api/user/saved-routes` | JWT | Save a route |

## First EF Migration

After restoring packages, create the initial migration:

```bash
cd backend/RouteEats.Api
dotnet ef migrations add InitialCreate
dotnet ef database update
```
