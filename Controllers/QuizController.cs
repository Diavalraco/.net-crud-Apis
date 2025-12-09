using Microsoft.AspNetCore.Mvc;
using MyApi.DTOs;
using MyApi.Services;

namespace MyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class QuizController : ControllerBase
{
    private readonly IQuizService _quizService;
    private readonly ILogger<QuizController> _logger;

    public QuizController(IQuizService quizService, ILogger<QuizController> logger)
    {
        _quizService = quizService;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(QuizResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<QuizResponseDto>> CreateQuiz([FromBody] CreateQuizDto createQuizDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var quiz = await _quizService.CreateQuizAsync(createQuizDto);
            return CreatedAtAction(nameof(GetQuiz), new { id = quiz.Id }, quiz);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument when creating quiz");
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating quiz");
            return StatusCode(500, new { error = "An error occurred while creating the quiz" });
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(QuizResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<QuizResponseDto>> GetQuiz(string id)
    {
        try
        {
            var quiz = await _quizService.GetQuizByIdAsync(id);
            if (quiz == null)
            {
                return NotFound(new { error = $"Quiz with ID {id} not found" });
            }

            return Ok(quiz);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving quiz with ID {QuizId}", id);
            return StatusCode(500, new { error = "An error occurred while retrieving the quiz" });
        }
    }


    [HttpPut("{id}")]
    [ProducesResponseType(typeof(QuizResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<QuizResponseDto>> UpdateQuiz(string id, [FromBody] UpdateQuizDto updateQuizDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var quiz = await _quizService.UpdateQuizAsync(id, updateQuizDto);
            if (quiz == null)
            {
                return NotFound(new { error = $"Quiz with ID {id} not found" });
            }

            return Ok(quiz);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument when updating quiz");
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating quiz with ID {QuizId}", id);
            return StatusCode(500, new { error = "An error occurred while updating the quiz" });
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteQuiz(string id)
    {
        try
        {
            var deleted = await _quizService.DeleteQuizAsync(id);
            if (!deleted)
            {
                return NotFound(new { error = $"Quiz with ID {id} not found" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting quiz with ID {QuizId}", id);
            return StatusCode(500, new { error = "An error occurred while deleting the quiz" });
        }
    }
}

