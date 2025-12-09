# Quiz API - ASP.NET Core Application

A RESTful API for managing quiz questions with full CRUD operations, built with ASP.NET Core and MongoDB.

## Project Structure

```
MyApi/
├── Controllers/          # API Controllers
│   └── QuizController.cs
├── Data/                 # Data Access Layer
│   ├── IQuizRepository.cs
│   ├── QuizRepository.cs
│   └── MongoDbSettings.cs
├── DTOs/                 # Data Transfer Objects
│   ├── CreateQuizDto.cs
│   ├── UpdateQuizDto.cs
│   └── QuizResponseDto.cs
├── Models/               # Domain Models
│   └── Quiz.cs
├── Services/             # Business Logic Layer
│   ├── IQuizService.cs
│   └── QuizService.cs
├── Properties/           # Application Properties
├── appsettings.json     # Configuration
└── Program.cs           # Application Entry Point
```

## Features

- **4 CRUD Endpoints:**
  - `POST /api/quiz` - Create a new quiz question
  - `GET /api/quiz/{id}` - Get a quiz question by ID
  - `PUT /api/quiz/{id}` - Update an existing quiz question
  - `DELETE /api/quiz/{id}` - Delete a quiz question

- **Industry-Grade Architecture:**
  - Clean separation of concerns (MVC pattern)
  - Repository pattern for data access
  - Service layer for business logic
  - DTOs for request/response mapping
  - Dependency Injection
  - Input validation
  - Error handling

## Prerequisites

- .NET 10.0 SDK
- MongoDB Atlas account (connection string already configured)

## Setup

1. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

2. Build the project:
   ```bash
   dotnet build
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

The API will be available at:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger UI: `https://localhost:5001/swagger`

## Testing with Postman

1. Import the `Postman_Collection.json` file into Postman
2. Set the `baseUrl` variable in Postman to your API URL (default: `https://localhost:5001`)
3. Test the endpoints in this order:
   - **Create Quiz**: Create a new quiz question (save the returned `id`)
   - **Get Quiz by ID**: Use the `id` from the create response
   - **Update Quiz**: Update the quiz using the same `id`
   - **Delete Quiz**: Delete the quiz using the same `id`

## API Endpoints

### 1. Create Quiz
**POST** `/api/quiz`

Request Body:
```json
{
    "question": "What is the capital of France?",
    "options": ["London", "Berlin", "Paris", "Madrid"],
    "correctAnswer": 2,
    "category": "Geography",
    "difficulty": "Easy"
}
```

Response: `201 Created`
```json
{
    "id": "507f1f77bcf86cd799439011",
    "question": "What is the capital of France?",
    "options": ["London", "Berlin", "Paris", "Madrid"],
    "correctAnswer": 2,
    "category": "Geography",
    "difficulty": "Easy",
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-01T00:00:00Z"
}
```

### 2. Get Quiz by ID
**GET** `/api/quiz/{id}`

Response: `200 OK` (same structure as Create response)

### 3. Update Quiz
**PUT** `/api/quiz/{id}`

Request Body (all fields optional):
```json
{
    "question": "Updated question?",
    "difficulty": "Medium"
}
```

Response: `200 OK` (updated quiz object)

### 4. Delete Quiz
**DELETE** `/api/quiz/{id}`

Response: `204 No Content`

## Validation Rules

- **Question**: Required, 5-500 characters
- **Options**: Required, 2-6 options
- **CorrectAnswer**: Required, must be a valid index (0 to options.length - 1)
- **Category**: Required, 2-100 characters
- **Difficulty**: Required, must be "Easy", "Medium", or "Hard"

## Error Responses

- `400 Bad Request`: Invalid input data
- `404 Not Found`: Quiz not found
- `500 Internal Server Error`: Server error

