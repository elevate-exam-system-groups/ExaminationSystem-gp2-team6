using AutoMapper;
using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.QuizEngine.StartQuiz.Orchestrator;
using ExaminationSystem.Features.QuizEngine.StartQuiz.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.QuizEngine.StartQuiz.CreateQuiz
{
    [ApiController]
    [Route("api/quizzes/{id}/start")]
    [Authorize]
    public class StartQuizEndPoint : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> StartQuiz(
            [FromBody] StartQuizRequestViewModel request,
            [FromServices] IMediator mediator,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new StartQuizOrchestrator(request.QuizId, request.StudentId),
                cancellationToken);

            if (!result.IsSuccess)
                return result.ErrorCode switch
                {
                    ErrorCode.NotFound => NotFound(new { result.Message }),
                    ErrorCode.AttemptLimitReached => Conflict(new { result.Message }),
                    _ => BadRequest(new { result.Message })
                };

            var viewModel = mapper.Map<StartQuizResponseViewModel>(
                result.Data,
                opt =>
                {
                    opt.Items["QuizId"] = request.QuizId;
                    opt.Items["StudentId"] = request.StudentId;
                });

            return Ok(RequestResult<StartQuizResponseViewModel>.Success(viewModel));
        }
    }
}
