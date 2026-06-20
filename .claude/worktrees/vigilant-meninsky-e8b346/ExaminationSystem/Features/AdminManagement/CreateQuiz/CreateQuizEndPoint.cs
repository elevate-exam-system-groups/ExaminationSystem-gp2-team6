using ExaminationSystem.Common.Data;
using ExaminationSystem.Features.AdminManagement.CreateQuiz.Orchestrator;
using ExaminationSystem.Features.AdminManagement.CreateQuiz.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.AdminManagement.CreateQuiz
{
    [ApiController]
    [Route("api/admin/quizzes")]
    [Authorize(Roles = "Admin")]
    public class CreateQuizEndPoint : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateQuiz(
            [FromBody] CreateQuizRequestViewModel request,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new CreateQuizOrchestrator(
                    request.Title, request.DiplomaId, request.DurationMinutes,
                    request.PassScore, request.MaxAttempts, request.Instructions),
                cancellationToken);

            if (!result.IsSuccess)
                return result.ErrorCode switch
                {
                    ErrorCode.NotFound => NotFound(new { result.Message }),
                    ErrorCode.AlreadyExists => Conflict(new { result.Message }),
                    _ => BadRequest(new { result.Message })
                };

            return StatusCode(StatusCodes.Status201Created, new { result.Message });
        }
    }
}
