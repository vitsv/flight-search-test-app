# Flight Search — Demo App

A full-stack flight search prototype built as a coding interview exercise, with assistance from [Claude](https://claude.ai) (Anthropic AI).

- **Frontend**: Vue 3 + Vite + Vuetify 3
- **Backend**: ASP.NET Core Web API (.NET 10)

## Repository Structure

```
flight-search-test-app/
├── src/
│   ├── frontend/               # Vue 3 app
│   │   ├── src/
│   │   │   ├── components/
│   │   │   │   ├── FlightSearchForm.vue
│   │   │   │   └── SearchSummary.vue
│   │   │   ├── api.js          # Axios client
│   │   │   ├── App.vue
│   │   │   └── main.js
│   │   └── .env                # VITE_API_BASE_URL
│   └── backend/
│       └── FlightSearch.Api/
│           ├── Controllers/    # AirportsController
│           ├── Data/           # AirportRepository + airports.json
│           ├── Middleware/     # RequestLoggingMiddleware
│           ├── Models/         # Airport record
│           └── Program.cs
├── USER_STORIES.md
└── README.md
```

## Prerequisites

| Tool | Version |
|------|---------|
| .NET SDK | 10.0+ |
| Node.js | 20+ |
| npm | 10+ |

## Running Locally

Start the backend first, then the frontend in a separate terminal.

### 1. Backend

```bash
cd src/backend/FlightSearch.Api
dotnet run
```

The API starts at `http://localhost:5000`. Logs are written to the console and to daily rolling files under `src/backend/FlightSearch.Api/logs/`.

### 2. Frontend

```bash
cd src/frontend
npm install
npm run dev
```

The app opens at `http://localhost:5173`. The API base URL is configured in `src/frontend/.env`:

```
VITE_API_BASE_URL=http://localhost:5000
```

## Features

| Feature | Status |
|---------|--------|
| One-way / Return toggle | ✓ |
| Origin dropdown (loaded from API) | ✓ |
| Destination dropdown (updates on origin change) | ✓ |
| Loading and error states | ✓ |
| Search summary display | ✓ |
| Departure / Return date pickers | ✓ bonus |
| Passenger count stepper | ✓ bonus |
| Responsive layout | ✓ |

## API Reference

### List all origin airports

```
GET /api/airports
```

**Response `200 OK`:**
```json
[
  { "code": "CDG", "name": "Paris Charles de Gaulle" },
  { "code": "DXB", "name": "Dubai International" },
  { "code": "FRA", "name": "Frankfurt Airport" },
  { "code": "JFK", "name": "New York JFK" },
  { "code": "LHR", "name": "London Heathrow" },
  { "code": "SIN", "name": "Singapore Changi" }
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
| `origin` | path | string | IATA airport code, e.g. `LHR` (case-insensitive) |

**Responses:**

| Status | Description |
|--------|-------------|
| `200 OK` | JSON array of destination airport objects |
| `400 Bad Request` | Code is empty, shorter than 2 chars, or contains non-letter characters |
| `404 Not Found` | Code is not in the dataset |

**Example — `GET /api/airports/destinations/LHR`**
```json
[
  { "code": "CDG", "name": "Paris Charles de Gaulle" },
  { "code": "DXB", "name": "Dubai International" },
  { "code": "FRA", "name": "Frankfurt Airport" },
  { "code": "JFK", "name": "New York JFK" },
  { "code": "SIN", "name": "Singapore Changi" }
]
```

**Example — `GET /api/airports/destinations/XYZ` → `404`**
```json
{ "error": "Origin airport 'XYZ' was not found." }
```

**Example — `GET /api/airports/destinations/1` → `400`**
```json
{ "error": "Origin code must contain only letters and be at least 2 characters long." }
```

## Logging

Requests are logged by `RequestLoggingMiddleware` at three levels:

| Response status | Log level |
|----------------|-----------|
| 2xx | `Information` |
| 4xx | `Warning` |
| 5xx | `Error` |

Each entry includes timestamp, HTTP method, path, and status code. The controller additionally logs the `origin` parameter and result count on every call.

Example log output:
```
[2026-05-30 10:15:32 INF] GET /api/airports/destinations/LHR → 200
[2026-05-30 10:15:33 WRN] GET /api/airports/destinations/XYZ → 404
```

Log files: `src/backend/FlightSearch.Api/logs/flight-search-YYYYMMDD.log`

## Notes

- Airport data is loaded from `airports.json` at startup — no database required.
- CORS is configured to allow `http://localhost:5173` (Vite dev server).
- The `VITE_API_BASE_URL` env var can be changed to point at any backend host.
