using MyApi.DTOs;
using MyApi.Models;

namespace MyApi.Services;

public interface IQuizService
{
    Task<QuizResponseDto> CreateQuizAsync(CreateQuizDto createQuizDto);
    Task<QuizResponseDto?> GetQuizByIdAsync(string id);
    Task<QuizResponseDto?> UpdateQuizAsync(string id, UpdateQuizDto updateQuizDto);
    Task<bool> DeleteQuizAsync(string id);
}

