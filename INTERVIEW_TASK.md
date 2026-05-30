# Test Application - Flight Search

Create a small flight search demo consisting of a **frontend** and a **backend API**. The front end is a simple search form; the backend is a .NET Web API that returns destination airports for a given origin and then returns flights for given criteria. Keep the implementation minimal, clear, and easy to run.

## Overview

1. **Goal**: Build a working prototype that demonstrates a clean frontend UI and a simple backend API that the frontend can call.
2. **Focus**: Code structure, readability, basic UX, logging, and error handling.
3. **Deliverables**: Source code in a public Git (GitHub or similar) repository, a short README with run instructions, and example requests.

## Frontend Requirements

1. **Page**: Flight search criteria page with:
   - **One-way / Return** toggle.
   - **Origin** and **Destination** airports dropdowns. Origin airport selection should drive available destinations by calling the backend API.
   - **Search** button that displays the selected values on the page.
2. **Bonus** (optional): Date pickers for departure/return and passenger count controls.
3. **Tech**: Use a modern JavaScript framework (preferably Vue.js) and a CSS/design framework for a polished look.
4. **Notes**: Keep UI simple and responsive. Show loading and error states when calling the API.

## Backend Requirements

1. **Service**: ASP.NET Core Web API (.NET 6+ recommended).
2. **Example endpoint**: `GET /api/airports/destinations/{origin}`
   - **Input**: origin airport code as path parameter.
   - **Success Response**: 200 OK with JSON array of destination codes, e.g. `["LON", "PAR", "LAX", "BLR"]`.
   - **Not Found**: 404 Not Found if origin is unknown.
   - **Bad Request**: 400 Bad Request for invalid input.
3. **Data Source**: In-memory dataset (hardcoded or JSON file). Real DB is optional.
4. **Logging**: Log each request and response with timestamp and origin parameter. Log errors.
5. **Error Handling**: Return meaningful HTTP status codes and messages.

## Evaluation Criteria

- **Correctness**: API returns expected destinations and frontend updates accordingly.
- **Code Quality**: Clear structure, readable code, clear git commits.
- **UX**: Simple, usable UI with loading and error states.
- **Logging and Error Handling**: Useful logs and graceful error responses.
- **Documentation**: Provide a clear README that explains how to run the app locally.
