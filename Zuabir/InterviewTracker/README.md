# Interview & Learning Tracker

A full-stack application to track your DSA problems, system design topics, mock interviews, and identify weak areas for improvement.

## Features

- **DSA Problem Tracking**: Track problems by category, difficulty, and status with spaced repetition reminders
- **System Design Topics**: Manage system design concepts with confidence levels
- **Mock Interview Feedback**: Record interview performance with detailed scoring
- **Weak Area Analytics**: Auto-detect and track areas needing improvement
- **Progress Dashboard**: Visual analytics and progress tracking

## Tech Stack

- **Backend**: .NET 8 Web API with Entity Framework Core & SQLite
- **Frontend**: Angular 17 with standalone components
- **Database**: SQLite (file-based, no setup required)

## Getting Started

### Prerequisites

- .NET 8 SDK
- Node.js 18+
- Angular CLI (`npm install -g @angular/cli`)

### Running the Backend

```bash
cd InterviewTracker.API
dotnet restore
dotnet run
```

The API will be available at `http://localhost:5000` with Swagger UI at `/swagger`.

### Running the Frontend

```bash
cd interview-tracker-ui
npm install
ng serve
```

The app will be available at `http://localhost:4200`.

## API Endpoints

### DSA Problems
- `GET /api/dsa` - Get all problems (with optional filters)
- `POST /api/dsa` - Create a new problem
- `PUT /api/dsa/{id}` - Update a problem
- `DELETE /api/dsa/{id}` - Delete a problem
- `POST /api/dsa/{id}/attempt` - Record an attempt
- `GET /api/dsa/needs-review` - Get problems due for review

### System Design
- `GET /api/systemdesign` - Get all topics
- `POST /api/systemdesign` - Create a new topic
- `PUT /api/systemdesign/{id}` - Update a topic
- `POST /api/systemdesign/{id}/review` - Record a review

### Mock Interviews
- `GET /api/interview` - Get all interviews
- `POST /api/interview` - Create a new interview
- `PUT /api/interview/{id}` - Update an interview

### Weak Areas
- `GET /api/weakarea` - Get weak areas
- `POST /api/weakarea` - Create a weak area
- `POST /api/weakarea/{id}/resolve` - Mark as resolved

### Analytics
- `GET /api/analytics/dashboard` - Dashboard stats
- `GET /api/analytics/dsa` - DSA performance analytics
- `GET /api/analytics/interviews` - Interview analytics
- `GET /api/analytics/weak-areas` - Weak area analysis

## Project Structure

```
InterviewTracker/
├── InterviewTracker.API/          # .NET Backend
│   ├── Controllers/               # API Controllers
│   ├── Data/                      # DbContext
│   ├── DTOs/                      # Data Transfer Objects
│   ├── Models/                    # Entity Models
│   └── Program.cs                 # Entry point
│
└── interview-tracker-ui/          # Angular Frontend
    └── src/
        └── app/
            ├── pages/             # Page components
            │   ├── dashboard/
            │   ├── dsa/
            │   ├── system-design/
            │   ├── interviews/
            │   ├── weak-areas/
            │   └── analytics/
            └── services/          # API service
```
