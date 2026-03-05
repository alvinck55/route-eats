# Project Brief: RouteEats
### Route-Aware Restaurant Discovery App

---

## Concept
A full-stack web application that suggests places to eat and drink **along a driving route**, filtered by user-selected categories (e.g. Sweet Treat, Fast Food, Meat, Coffee, etc.). The user inputs an origin and destination, selects their food preferences, and the app plots relevant restaurant suggestions directly on their route.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Frontend | React (SPA) |
| Map Rendering | Google Maps JS API via `@react-google-maps/api` |
| HTTP Client | Axios |
| Backend | ASP.NET Core Web API (C#) |
| Auth | ASP.NET Core Identity + JWT |
| ORM | Entity Framework Core |
| Database | PostgreSQL (via Docker) |
| External APIs | Google Directions API, Google Places API (Nearby Search) |
| Hosting | Azure (backend) + Vercel or Azure Static Web Apps (frontend) |

---

## Architecture Overview

```
React (SPA)  ←→  ASP.NET Core Web API  ←→  Google APIs + PostgreSQL
```

### Frontend (React)
- Single Page Application — loads once, React handles all routing/rendering
- Components: Search form, Map view, Restaurant cards, Category filter chips
- State management: React state (or Zustand if complexity grows)
- Communicates **only** with the ASP.NET backend — never directly with Google APIs

### Backend (ASP.NET Core)
- REST API with controllers handling incoming requests from React
- **Directions Service**: Calls Google Directions API, retrieves encoded polyline
- **Polyline Sampler**: Decodes polyline, samples coordinates at set intervals (e.g. every 15 miles)
- **Places Service**: Fires Google Places Nearby Search at each sampled coordinate
- **Deduplication + Category Mapper**: Removes duplicate results, maps Google place types to friendly app categories
- **Auth**: JWT-based login/registration via ASP.NET Core Identity
- **Entity Framework Core**: Manages all DB reads/writes (users, saved routes, preferences)

### Database (PostgreSQL)
- Users table (via ASP.NET Identity)
- Saved routes
- User preferences / favorite categories

---

## Core User Flow

```
1. User enters origin + destination + selects food categories
        ↓
2. [React] Sends POST /api/route/suggestions with route + categories
        ↓
3. [ASP.NET Controller] Receives and validates request
        ↓
4. [Directions Service] Calls Google Directions API → returns encoded polyline
        ↓
5. [Polyline Sampler] Decodes polyline, samples 8–10 coordinates along route
        ↓
6. [Places Service] Calls Google Places Nearby Search at each coordinate
        ↓
7. [Filter/Map Layer] Deduplicates results, maps to app categories
        ↓
8. [ASP.NET Controller] Returns clean JSON array of restaurant suggestions
        ↓
9. [React] Updates state, renders cards + map pins along the route
        ↓
10. User sees suggestions plotted on their route, can tap/click for details
```

---

## API Endpoints (Planned)

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/route/suggestions` | Main endpoint — accepts route + categories, returns suggestions |
| GET | `/api/categories` | Returns available food categories |
| POST | `/api/auth/register` | User registration |
| POST | `/api/auth/login` | Returns JWT token |
| GET | `/api/user/saved-routes` | Fetch saved routes (auth required) |
| POST | `/api/user/saved-routes` | Save a route (auth required) |

---

## Category Mapping (Google Places → App Categories)

| App Category | Google Place Types |
|---|---|
| Sweet Treat | `bakery`, `cafe`, `dessert_shop` |
| Fast Food | `meal_takeaway`, `fast_food_restaurant` |
| Meat / BBQ | `restaurant` + keyword filter "bbq", "steakhouse", "grill" |
| Coffee | `cafe`, `coffee_shop` |
| Sit-down Dinner | `restaurant` (excluding takeaway types) |

> **Note:** Category mapping will likely need refinement — Google's type system is imprecise. Consider an LLM classification layer as a future enhancement.

---

## Key Technical Challenges

1. **Polyline Sampling** — Decoding Google's encoded polyline format and sampling at distance intervals in C#
2. **API Cost Management** — Each route search triggers multiple Places API calls; implement caching for repeated routes
3. **Deduplication** — Nearby searches at overlapping points return duplicates; deduplicate by `place_id`
4. **Category Accuracy** — Google types don't map cleanly to friendly categories; requires a custom mapping layer

---

## Future Enhancements
- [ ] User accounts with saved routes and preferences
- [ ] Detour radius slider (how far off-route to search)
- [ ] "Open now" filter
- [ ] Rating threshold filter
- [ ] LLM-powered restaurant categorization
- [ ] Mobile-responsive design

---

## Development Environment
- **OS**: macOS (fully supported for .NET)
- **IDE**: VS Code with C# Dev Kit extension, or JetBrains Rider
- **Database**: PostgreSQL running via Docker Desktop
- **Runtime**: .NET 8 SDK

---

## Getting Started (First Steps for Claude Code)

1. Scaffold ASP.NET Core Web API project
2. Scaffold React frontend with Vite
3. Set up project folder structure (monorepo or separate repos)
4. Create the `RouteController` with a stub `/api/route/suggestions` endpoint
5. Integrate Google Maps JS in React with a basic map render
6. Build the Directions Service in C#
7. Build the Polyline Sampler utility
8. Build the Places Service with category filtering
9. Wire up frontend form → API call → map pin rendering
10. Add auth (Identity + JWT) once core flow works
