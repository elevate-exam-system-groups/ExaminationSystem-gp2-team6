using ExaminationSystem.Common.Exceptions;
using ExaminationSystem.Features.QuizEngine.ViewResults;
using ExaminationSystem.Features.QuizEngine.ViewResults.Dtos;
using ExaminationSystem.Features.QuizEngine.ViewResults.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExaminationSystem.Features.QuizEngine.SubmitQuiz.Commands
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttemptQuizEndpoints : ControllerBase
    {
        private readonly IMediator _mediator;

        public AttemptQuizEndpoints(IMediator mediator)
            => _mediator = mediator;

        [HttpPost("attempts/{attempt_id:int}/submit")]
        [ProducesResponseType(typeof(SubmitQuizResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Submit(int attempt_id, CancellationToken ct)
        {
            var studentId = GetCurrentUserId();

            try
            {
                var result = await _mediator.Send(
                    new SubmitQuizCommand(attempt_id, studentId), ct);

                return Ok(result);
            }
            catch (ConflictException ex)
            {
                return Conflict(new { message = ex.Message, existingResult = ex.ExistingResult });
            }
            catch (ForbiddenAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("attempts/{attempt_id:int}/results")]
        [ProducesResponseType(typeof(AttemptResultsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetResults(int attempt_id, CancellationToken ct)
        {
            var requesterId = GetCurrentUserId();
            bool isAdmin = User.IsInRole("Admin");

            try
            {
                var dto = await _mediator.Send(
                    new GetAttemptResultsQuery(attempt_id, requesterId, isAdmin), ct);

                return Ok(dto);
            }
            catch (ForbiddenAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("student/attempts")]
        [ProducesResponseType(typeof(ExaminationSystem.Common.Pagination.PaginatedResult<AttemptSummaryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetHistory(
            [FromQuery] int? quiz_id,
            [FromQuery] int? diploma_id,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken ct = default)
        {
            var studentId = GetCurrentUserId();

            var result = await _mediator.Send(
                new GetStudentAttemptsQuery(studentId, quiz_id, diploma_id, page, pageSize), ct);

            return Ok(result);
        }

        [HttpGet("student/attempts/{attempt_id:int}")]
        [ProducesResponseType(typeof(AttemptResultsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAttemptDetail(int attempt_id, CancellationToken ct)
        {
            var requesterId = GetCurrentUserId();

            try
            {
                var dto = await _mediator.Send(
                    new GetAttemptResultsQuery(attempt_id, requesterId, RequesterIsAdmin: false), ct);

                return Ok(dto);
            }
            catch (ForbiddenAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        private Guid GetCurrentUserId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("User identity not found in token.");

            if (!Guid.TryParse(claim, out var userId))
            {
                throw new UnauthorizedAccessException("User identity is invalid.");
            }

            return userId;
        }
    }
}
