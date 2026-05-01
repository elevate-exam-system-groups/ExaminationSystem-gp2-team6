using ExaminationSystem.Common.Data;
using ExaminationSystem.Features.QuizEngine.AnswerQuestions.Orchestrator;
using ExaminationSystem.Features.QuizEngine.AnswerQuestions.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.QuizEngine.AnswerQuestions
{
    [ApiController]
    [Route("api/quiz")]
    [Authorize]
    public class AnswerQuestionEndPoint : ControllerBase
    {
        [HttpPost("answer")]
        public async Task<IActionResult> AnswerQuestion(
            [FromBody] AnswerQuestionRequestViewModel request,
            CancellationToken cancellationToken,
            [FromServices] IMediator mediator)
        {
            var result = await mediator.Send(
                new AnswerQuestionOrchestrator(
                    request.AttemptId, request.QuestionId, request.OptionId, request.StudentId),
                cancellationToken);

            if (!result.IsSuccess)
                return result.ErrorCode switch
                {
                    ErrorCode.NotFound => NotFound(new { result.Message }),
                    ErrorCode.Forbidden => StatusCode(StatusCodes.Status403Forbidden, new { result.Message }),
                    ErrorCode.Conflict => Conflict(new { result.Message }),
                    ErrorCode.Gone => StatusCode(StatusCodes.Status410Gone, new { result.Message }),
                    _ => BadRequest(new { result.Message })
                };

            return Ok(new { message = "Answer saved successfully." });
        }
    }
}
