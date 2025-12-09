using MongoDB.Driver;
using MyApi.Models;

namespace MyApi.Data;

public class QuizRepository : IQuizRepository
{
    private readonly IMongoCollection<Quiz> _quizzes;

    public QuizRepository(IMongoDatabase database)
    {
        _quizzes = database.GetCollection<Quiz>("quizzes");
    }

    public async Task<Quiz?> GetByIdAsync(string id)
    {
        return await _quizzes.Find(q => q.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Quiz> CreateAsync(Quiz quiz)
    {
        await _quizzes.InsertOneAsync(quiz);
        return quiz;
    }

    public async Task<Quiz?> UpdateAsync(string id, Quiz quiz)
    {
        quiz.UpdatedAt = DateTime.UtcNow;
        var result = await _quizzes.ReplaceOneAsync(q => q.Id == id, quiz);
        return result.IsAcknowledged && result.ModifiedCount > 0 ? quiz : null;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _quizzes.DeleteOneAsync(q => q.Id == id);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }
}

