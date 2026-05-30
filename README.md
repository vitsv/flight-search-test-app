# Flight Search — Demo App

A full-stack flight search prototype built for a coding interview.

- **Frontend**: Vue 3 + Vite + Vuetify
- **Backend**: ASP.NET Core Web API (.NET 8)

## Repository Structure

```
flight-search-test-app/
├── src/
│   ├── frontend/   # Vue 3 app
│   └── backend/    # ASP.NET Core Web API
├── USER_STORIES.md
└── README.md
```

## Prerequisites

| Tool | Version |
|------|---------|
| .NET SDK | 8.0+ |
| Node.js | 20+ |
| npm | 10+ |

## Running Locally

### 1. Backend

```bash
cd src/backend/FlightSearch.Api
dotnet run
```

API will be available at `http://localhost:5000`.

### 2. Frontend

```bash
cd src/frontend
npm install
npm run dev
```

App will be available at `http://localhost:5173`.

## API Reference

### List all origin airports

```
GET /api/airports
```

**Response `200 OK`:**
```json
[
  { "code": "LHR", "name": "London Heathrow" },
  { "code": "CDG", "name": "Paris Charles de Gaulle" }
]
```

---

### Get destinations for an origin

```
GET /api/airports/destinations/{origin}
```

**Parameters:**

| Name | In | Type | Description |
|------|----|------|-------------|
| origin | path | string | IATA airport code (e.g. `LHR`) |

**Responses:**

| Status | Description |
|--------|-------------|
| 200 OK | JSON array of destination airport objects |
| 400 Bad Request | Origin code is invalid (empty or non-alphabetic) |
| 404 Not Found | Origin code is not in the dataset |

**Example — `GET /api/airports/destinations/LHR`:**
```json
[
  { "code": "CDG", "name": "Paris Charles de Gaulle" },
  { "code": "JFK", "name": "New York JFK" },
  { "code": "DXB", "name": "Dubai International" }
]
```

**Example — `404`:**
```json
{ "error": "Origin airport 'XYZ' was not found." }
```

**Example — `400`:**
```json
{ "error": "Origin code must contain only letters and be at least 2 characters long." }
```

## Notes

- Data is in-memory (hardcoded). No database setup is required.
- CORS is configured to allow requests from `http://localhost:5173` (Vite dev server).
