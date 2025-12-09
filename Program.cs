using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MyApi.Data;
using MyApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Configure MongoDB - Read from environment variables first, then configuration
var connectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING") 
    ?? builder.Configuration["MongoDbSettings:ConnectionString"];
var databaseName = Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME") 
    ?? builder.Configuration["MongoDbSettings:DatabaseName"];

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("MongoDB connection string is not configured. Please set MONGODB_CONNECTION_STRING environment variable or configure it in appsettings.json");
}

if (string.IsNullOrEmpty(databaseName))
{
    throw new InvalidOperationException("MongoDB database name is not configured. Please set MONGODB_DATABASE_NAME environment variable or configure it in appsettings.json");
}

var mongoClient = new MongoClient(connectionString);
var mongoDatabase = mongoClient.GetDatabase(databaseName);

// Register services
builder.Services.AddSingleton<IMongoDatabase>(mongoDatabase);
builder.Services.AddScoped<IQuizRepository, QuizRepository>();
builder.Services.AddScoped<IQuizService, QuizService>();

// Add API versioning and validation
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .SelectMany(x => x.Value!.Errors)
            .Select(x => x.ErrorMessage)
            .ToList();

        return new BadRequestObjectResult(new { errors });
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
