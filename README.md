# Markel Claims tech test

An app for the tech test using a simple Angular frontend with some basic styling added, with a .NET 10 backend for the Api controllers, using Mediatr library to run the CQRS pattern to keep the queries/commands to the in-store Db simple and clean.

The UI returns the GET/PUT data on screen, but also added a save to JSON file option as that's in the specs.

The API key is handled on the UI side for every request so no need to dig for an Api key to paste into the browser and whatnot.

It also includes unit test coverage for the Api.

Created using Visual studio code and Cursor for agentic assistance for planning and refining design patterns and architecture.

## To Run, open two terminals, one for UI, one for API..
API (from the repo root):

```bash
dotnet run --project src/Markel.Api --launch-profile http
```

The HTTP profile listens on `http://localhost:5042`. Swagger is at `/swagger` though this would be for API only demo'ing.

Angular (from `frontend/`):

```bash
npm start
```

The Angular dev server proxies `/api` to `http://localhost:5042`.

Tests:

```bash
dotnet test
```

## API key

Every `/api` request requires:

```
X-Api-Key: tech-test-local-key
```

The value is in `src/Markel.Api/appsettings.json` (`ApiKey`) and `frontend/src/environments/environment.ts`. Paste the same key into Swagger’s Authorize dialog.

## Endpoints

| Method | Route | Notes |
| --- | --- | --- |
| GET | `/api/companies` | Company list, including `hasActiveInsurancePolicy` |
| GET | `/api/companies/{id}` | Single company, including `hasActiveInsurancePolicy` |
| GET | `/api/companies/{id}/claims` | Claims for one company |
| GET | `/api/claims/{ucr}` | Single claim, including `ageInDays` |
| PUT | `/api/claims/{ucr}` | Update a claim (not UCR or company id) |

Update body:

```json
{
  "claimDate": "2026-03-10",
  "lossDate": "2026-03-08",
  "assuredName": "Ethan's awesome company",
  "incurredLoss": 12500.50,
  "closed": false
}
```

Missing resources return 404 ProblemDetails. Validation failures return 400. A missing or wrong API key returns 401.

## Computed fields

- `hasActiveInsurancePolicy` is true when `active` is true and `insuranceEndDate` is today or later (UTC date).
- `ageInDays` is the whole number of days from `claimDate` to today (UTC), not `lossDate`.

## Architecture

Clean Architecture with CQRS and MediatR:

- `Markel.Domain` — entities and repository interfaces
- `Markel.Application` — queries, commands, validators, DTOs
- `Markel.Infrastructure` — in-memory store and seed data (no SQL Server)
- `Markel.Api` — controllers, API key, Swagger, CORS, ProblemDetails

`ClaimType` is seeded in the store to match the given database script. Claims have no `ClaimTypeId` in that script, so the type is not exposed on the API.

Data is held in memory for the lifetime of the API process.
