using MyApi.Data;
using MyApi.DTOs;
using MyApi.Models;

namespace MyApi.Services;

public class QuizService : IQuizService
{
    private readonly IQuizRepository _quizRepository;

    public QuizService(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<QuizResponseDto> CreateQuizAsync(CreateQuizDto createQuizDto)
    {
       
        if (createQuizDto.CorrectAnswer < 0 || createQuizDto.CorrectAnswer >= createQuizDto.Options.Count)
        {
            throw new ArgumentException("Correct answer index is out of range for the provided options.");
        }

        var quiz = new Quiz
        {
            Question = createQuizDto.Question,
            Options = createQuizDto.Options,
            CorrectAnswer = createQuizDto.CorrectAnswer,
            Category = createQuizDto.Category,
            Difficulty = createQuizDto.Difficulty,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createdQuiz = await _quizRepository.CreateAsync(quiz);
        return MapToResponseDto(createdQuiz);
    }

    public async Task<QuizResponseDto?> GetQuizByIdAsync(string id)
    {
        var quiz = await _quizRepository.GetByIdAsync(id);
        return quiz != null ? MapToResponseDto(quiz) : null;
    }

    public async Task<QuizResponseDto?> UpdateQuizAsync(string id, UpdateQuizDto updateQuizDto)
    {
        var existingQuiz = await _quizRepository.GetByIdAsync(id);
        if (existingQuiz == null)
        {
            return null;
        }

        
        if (!string.IsNullOrWhiteSpace(updateQuizDto.Question))
        {
            existingQuiz.Question = updateQuizDto.Question;
        }

        if (updateQuizDto.Options != null && updateQuizDto.Options.Count > 0)
        {
            existingQuiz.Options = updateQuizDto.Options;
        }

        if (updateQuizDto.CorrectAnswer.HasValue)
        {
            var options = existingQuiz.Options;
            if (updateQuizDto.CorrectAnswer.Value < 0 || updateQuizDto.CorrectAnswer.Value >= options.Count)
            {
                throw new ArgumentException("Correct answer index is out of range for the provided options.");
            }
            existingQuiz.CorrectAnswer = updateQuizDto.CorrectAnswer.Value;
        }

        if (!string.IsNullOrWhiteSpace(updateQuizDto.Category))
        {
            existingQuiz.Category = updateQuizDto.Category;
        }

        if (!string.IsNullOrWhiteSpace(updateQuizDto.Difficulty))
        {
            existingQuiz.Difficulty = updateQuizDto.Difficulty;
        }

        existingQuiz.UpdatedAt = DateTime.UtcNow;

        var updatedQuiz = await _quizRepository.UpdateAsync(id, existingQuiz);
        return updatedQuiz != null ? MapToResponseDto(updatedQuiz) : null;
    }

    public async Task<bool> DeleteQuizAsync(string id)
    {
        return await _quizRepository.DeleteAsync(id);
    }

    private static QuizResponseDto MapToResponseDto(Quiz quiz)
    {
        return new QuizResponseDto
        {
            Id = quiz.Id ?? string.Empty,
            Question = quiz.Question,
            Options = quiz.Options,
            CorrectAnswer = quiz.CorrectAnswer,
            Category = quiz.Category,
            Difficulty = quiz.Difficulty,
            CreatedAt = quiz.CreatedAt,
            UpdatedAt = quiz.UpdatedAt
        };
    }
}

