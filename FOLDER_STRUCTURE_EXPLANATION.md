# 📁 Complete Folder Structure Explanation

## 🏗️ Project Architecture Overview

This ASP.NET Core Quiz API follows the **MVC (Model-View-Controller) pattern** with additional layers for separation of concerns:

```
Request Flow:
Client → Controller → Service → Repository → MongoDB
                ↓         ↓          ↓
              DTOs    Business   Data Access
                      Logic
```

---

## 📂 Root Level Files

### `Program.cs`
**Purpose**: Application entry point and configuration
- Configures dependency injection (DI) container
- Sets up MongoDB connection
- Registers all services (Repository, Service, Controllers)
- Configures middleware pipeline (Swagger, HTTPS, Authorization)
- Starts the web server

**Key Responsibilities**:
- Service registration (lines 9-26)
- MongoDB setup (lines 14-24)
- Middleware configuration (lines 46-54)

---

### `MyApi.csproj`
**Purpose**: Project configuration file
- Defines target framework (.NET 10.0)
- Lists NuGet package dependencies:
  - `Microsoft.AspNetCore.OpenApi` - OpenAPI support
  - `MongoDB.Driver` - MongoDB database driver
  - `Swashbuckle.AspNetCore` - Swagger/OpenAPI UI

---

### `appsettings.json` & `appsettings.Development.json`
**Purpose**: Application configuration
- **appsettings.json**: Production settings
  - MongoDB connection string
  - Database name
  - Logging configuration
- **appsettings.Development.json**: Development overrides
  - More verbose logging in development

---

### `Properties/launchSettings.json`
**Purpose**: Launch profiles for different environments
- Defines ports (HTTP: 5232, HTTPS: 7191)
- Environment variables
- Browser launch settings

---

## 📁 Controllers/ Folder

### `QuizController.cs`
**Purpose**: Handles HTTP requests and responses (API endpoints)

**Responsibilities**:
- Receives HTTP requests from clients
- Validates input using ModelState
- Calls service layer for business logic
- Returns HTTP responses (200, 201, 400, 404, 500)
- Error handling and logging

**4 CRUD Endpoints**:
1. `POST /api/quiz` - Create new quiz
2. `GET /api/quiz/{id}` - Get quiz by ID
3. `PUT /api/quiz/{id}` - Update quiz
4. `DELETE /api/quiz/{id}` - Delete quiz

**Key Features**:
- `[ApiController]` - Enables API-specific behaviors
- `[Route("api/[controller]")]` - Sets base route to `/api/quiz`
- Dependency injection of `IQuizService`
- Proper HTTP status codes
- XML documentation comments for Swagger

---

## 📁 Models/ Folder

### `Quiz.cs`
**Purpose**: Domain entity representing a quiz question in the database

**Structure**:
- **Id**: MongoDB ObjectId (auto-generated)
- **Question**: The quiz question text
- **Options**: List of answer choices
- **CorrectAnswer**: Index (0-based) of correct option
- **Category**: Quiz category (e.g., "Geography", "Science")
- **Difficulty**: "Easy", "Medium", or "Hard"
- **CreatedAt/UpdatedAt**: Timestamps

**MongoDB Attributes**:
- `[BsonId]` - Marks primary key
- `[BsonElement("fieldName")]` - Maps C# property to MongoDB field
- `[BsonRepresentation(BsonType.ObjectId)]` - Converts string to ObjectId

**Why**: This is the **data model** that matches your MongoDB collection structure.

---

## 📁 DTOs/ Folder (Data Transfer Objects)

**Purpose**: Objects used for API communication (not database storage)

### `CreateQuizDto.cs`
**Purpose**: Input model for creating a quiz
- Used in `POST /api/quiz` request body
- Contains validation attributes:
  - `[Required]` - Field must be provided
  - `[StringLength]` - Min/max length validation
  - `[Range]` - Numeric range validation
  - `[RegularExpression]` - Pattern matching (for Difficulty)

**Why DTOs?**: 
- Separates API contract from database model
- Allows different validation rules
- Can hide sensitive fields
- Easier to version APIs

### `UpdateQuizDto.cs`
**Purpose**: Input model for updating a quiz
- All fields are **optional** (nullable)
- Only provided fields are updated
- Same validation as CreateQuizDto

### `QuizResponseDto.cs`
**Purpose**: Output model returned to clients
- Clean structure for API responses
- Excludes internal fields if needed
- Consistent response format

**DTO Pattern Benefits**:
- **Security**: Don't expose internal IDs or fields
- **Flexibility**: Change database model without breaking API
- **Validation**: Different rules for create vs update

---

## 📁 Services/ Folder (Business Logic Layer)

### `IQuizService.cs`
**Purpose**: Interface defining business operations
- Contract that `QuizService` implements
- Enables dependency injection and testing

### `QuizService.cs`
**Purpose**: Contains business logic and orchestration

**Responsibilities**:
- Validates business rules (e.g., correct answer index)
- Maps between DTOs and Models
- Calls repository for data operations
- Handles business exceptions

**Key Methods**:
- `CreateQuizAsync()` - Validates and creates quiz
- `GetQuizByIdAsync()` - Retrieves quiz
- `UpdateQuizAsync()` - Updates quiz (partial updates)
- `DeleteQuizAsync()` - Deletes quiz
- `MapToResponseDto()` - Converts Model to DTO

**Why Service Layer?**:
- Separates business logic from controllers
- Reusable across different controllers
- Easier to test
- Single responsibility principle

---

## 📁 Data/ Folder (Data Access Layer)

### `MongoDbSettings.cs`
**Purpose**: Configuration class for MongoDB settings
- Maps to `appsettings.json` section
- Contains ConnectionString and DatabaseName

### `IQuizRepository.cs`
**Purpose**: Interface defining data access operations
- Defines contract for database operations
- Enables dependency injection and mocking for tests

### `QuizRepository.cs`
**Purpose**: Implements MongoDB data access

**Responsibilities**:
- Direct interaction with MongoDB
- CRUD operations on `quizzes` collection
- Uses MongoDB driver (`IMongoCollection<T>`)

**Key Methods**:
- `GetByIdAsync()` - Finds quiz by ObjectId
- `CreateAsync()` - Inserts new quiz
- `UpdateAsync()` - Replaces existing quiz
- `DeleteAsync()` - Removes quiz

**Repository Pattern Benefits**:
- Abstracts database implementation
- Easy to swap databases (MongoDB → SQL Server)
- Testable (can mock repository)
- Centralized data access logic

---

## 📁 bin/ & obj/ Folders

### `bin/` (Binary Output)
**Purpose**: Compiled application files
- Contains `.dll` files (compiled code)
- Runtime dependencies
- Configuration files
- **Generated automatically** - Don't commit to Git

### `obj/` (Object Files)
**Purpose**: Intermediate build files
- Temporary compilation artifacts
- Cache files for faster builds
- **Generated automatically** - Don't commit to Git

---

## 📁 Properties/ Folder

### `launchSettings.json`
**Purpose**: IDE and runtime launch configuration
- Defines development ports
- Environment variables
- Browser launch settings
- Multiple profiles (HTTP, HTTPS)

---

## 🔄 Request Flow Example

### Example: Creating a Quiz

```
1. Client sends POST /api/quiz
   ↓
2. QuizController.CreateQuiz() receives CreateQuizDto
   ↓
3. Controller validates ModelState
   ↓
4. Controller calls QuizService.CreateQuizAsync()
   ↓
5. Service validates business rules (correct answer index)
   ↓
6. Service maps CreateQuizDto → Quiz Model
   ↓
7. Service calls QuizRepository.CreateAsync()
   ↓
8. Repository inserts Quiz into MongoDB "quizzes" collection
   ↓
9. Repository returns Quiz with generated Id
   ↓
10. Service maps Quiz → QuizResponseDto
    ↓
11. Controller returns 201 Created with QuizResponseDto
    ↓
12. Client receives response
```

---

## 🎯 Design Patterns Used

1. **MVC Pattern**: Separation of Controller, Model, and View (DTOs)
2. **Repository Pattern**: Abstracts data access
3. **Service Layer Pattern**: Business logic separation
4. **DTO Pattern**: Data transfer objects for API
5. **Dependency Injection**: Loose coupling between layers
6. **Interface Segregation**: Interfaces for all major components

---

## 📊 Layer Responsibilities Summary

| Layer | Responsibility | Example |
|-------|---------------|---------|
| **Controller** | HTTP handling, validation, responses | `QuizController` |
| **Service** | Business logic, orchestration | `QuizService` |
| **Repository** | Data access, database operations | `QuizRepository` |
| **Model** | Domain entity, database structure | `Quiz` |
| **DTO** | API contract, validation | `CreateQuizDto` |

---

## ✅ Best Practices Implemented

1. ✅ **Separation of Concerns**: Each layer has one responsibility
2. ✅ **Dependency Injection**: All dependencies injected via constructor
3. ✅ **Interface-Based Design**: Interfaces for testability
4. ✅ **Input Validation**: Data annotations on DTOs
5. ✅ **Error Handling**: Try-catch blocks with proper HTTP status codes
6. ✅ **Logging**: ILogger for error tracking
7. ✅ **Async/Await**: All I/O operations are asynchronous
8. ✅ **RESTful Design**: Proper HTTP methods and status codes
9. ✅ **Documentation**: XML comments for Swagger
10. ✅ **Configuration**: Settings in appsettings.json

---

## 🚀 Why This Structure?

This structure is **industry-standard** because:

1. **Maintainable**: Easy to find and modify code
2. **Testable**: Each layer can be tested independently
3. **Scalable**: Easy to add new features
4. **Professional**: Follows ASP.NET Core best practices
5. **Team-Friendly**: Clear organization for multiple developers

