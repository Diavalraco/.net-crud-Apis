using MyApi.Models;

namespace MyApi.Data;

public interface IQuizRepository
{
    Task<Quiz?> GetByIdAsync(string id);
    Task<Quiz> CreateAsync(Quiz quiz);
    Task<Quiz?> UpdateAsync(string id, Quiz quiz);
    Task<bool> DeleteAsync(string id);
}

