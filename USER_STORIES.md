# User Stories — Flight Search App

## API (Backend)

### US-API-01: Retrieve destination airports
**As an** API consumer,  
**I want to** call `GET /api/airports/destinations/{origin}` with a valid origin code,  
**So that** I receive a list of available destination airport codes for that origin.

**Acceptance criteria:**
- Returns `200 OK` with a JSON array of destination codes (e.g. `["LON","PAR","LAX"]`).
- Origin code matching is case-insensitive.

---

### US-API-02: List all origin airports
**As an** API consumer,  
**I want to** call `GET /api/airports` to retrieve all available origin airports,  
**So that** I can populate the origin dropdown without hardcoding values in the frontend.

**Acceptance criteria:**
- Returns `200 OK` with a JSON array of airport objects containing at minimum `code` and `name`.

---

### US-API-03: Unknown origin returns 404
**As an** API consumer,  
**I want to** receive a `404 Not Found` when querying destinations for an unrecognised origin code,  
**So that** I can handle the missing-data case gracefully in the frontend.

**Acceptance criteria:**
- Returns `404 Not Found` with a descriptive error message in the body.

---

### US-API-04: Invalid input returns 400
**As an** API consumer,  
**I want to** receive a `400 Bad Request` when providing an invalid origin code (e.g. empty, too short, non-alphabetic),  
**So that** I know the input was malformed rather than simply not found.

**Acceptance criteria:**
- Returns `400 Bad Request` with a descriptive validation message.
- Triggered for codes shorter than 2 characters or containing non-letter characters.

---

### US-API-05: Request/response logging
**As an** operator,  
**I want** every API request and response to be logged with a timestamp, HTTP method, path, origin parameter, and response status code,  
**So that** I can monitor usage and diagnose issues.

**Acceptance criteria:**
- Each request produces at minimum one structured log entry containing: timestamp, method, path, origin value (where applicable), and response status.
- Errors (4xx, 5xx) are logged at `Warning` or `Error` level.

---

## Frontend

### US-FE-01: Trip type toggle
**As a** traveller,  
**I want to** toggle between **One-way** and **Return** trip modes,  
**So that** I can express the type of journey I am planning.

**Acceptance criteria:**
- Toggle is visible and clearly labelled.
- Selecting **Return** shows a return-date picker; selecting **One-way** hides it.

---

### US-FE-02: Origin airport selection
**As a** traveller,  
**I want to** pick my departure airport from a dropdown populated via the API,  
**So that** I can specify where I am flying from.

**Acceptance criteria:**
- Dropdown is populated on page load from `GET /api/airports`.
- Displays airport name and code.
- Shows a loading state while the list is fetching.

---

### US-FE-03: Destination auto-populate
**As a** traveller,  
**I want** the destination dropdown to update automatically whenever I change the origin,  
**So that** I only see destinations actually served from my chosen origin.

**Acceptance criteria:**
- Changing origin triggers a call to `GET /api/airports/destinations/{origin}`.
- Destination dropdown is disabled until an origin is selected.
- Previously selected destination is cleared when origin changes.

---

### US-FE-04: Loading state
**As a** traveller,  
**I want to** see a loading indicator while the API call for destinations is in progress,  
**So that** I know the application is working and have not encountered an error.

**Acceptance criteria:**
- A spinner or skeleton is visible on the destination dropdown during the API call.
- The Search button is disabled while any API call is in progress.

---

### US-FE-05: Error state
**As a** traveller,  
**I want to** see a clear error message when the API call for destinations fails,  
**So that** I know there is a problem and can try again.

**Acceptance criteria:**
- An inline error message is displayed below the destination dropdown on API failure.
- The error message disappears when the user selects a new origin or dismisses it.

---

### US-FE-06: Destination selection
**As a** traveller,  
**I want to** select my destination airport from the dropdown,  
**So that** I can specify where I am flying to.

**Acceptance criteria:**
- Dropdown is enabled only after an origin is selected and destinations have loaded.
- Displays airport code and name.

---

### US-FE-07: Search and display results
**As a** traveller,  
**I want to** click **Search** and see my selected criteria displayed on the page,  
**So that** I can confirm my selections before proceeding.

**Acceptance criteria:**
- Search button is enabled only when at minimum origin and destination are selected.
- Clicking Search shows a summary card/section with: trip type, origin, destination, and (if provided) dates and passenger count.

---

### US-FE-08: Departure date selection *(bonus)*
**As a** traveller,  
**I want to** pick a departure date using a date picker,  
**So that** I can specify when I plan to fly.

**Acceptance criteria:**
- Date picker prevents selection of past dates.
- Selected date is included in the search summary.

---

### US-FE-09: Return date selection *(bonus)*
**As a** traveller,  
**I want to** pick a return date when in Return mode,  
**So that** I can specify my full travel window.

**Acceptance criteria:**
- Return date picker is only visible in Return mode.
- Return date must be on or after the departure date.

---

### US-FE-10: Passenger count *(bonus)*
**As a** traveller,  
**I want to** set the number of passengers using a stepper control,  
**So that** the search reflects my group size.

**Acceptance criteria:**
- Minimum value is 1; maximum is 9.
- Selected count is included in the search summary.

---

### US-FE-11: Responsive layout
**As a** traveller on any device,  
**I want** the search form to be usable on both desktop and mobile screen sizes,  
**So that** I am not restricted to a specific device.

**Acceptance criteria:**
- Form is usable at viewport widths from 375 px upwards.
- No horizontal scrollbar on mobile.
