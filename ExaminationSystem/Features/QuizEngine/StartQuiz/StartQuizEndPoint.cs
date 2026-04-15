using AutoMapper;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.QuizEngine.StartQuiz.Orchestrator;
using ExaminationSystem.Features.QuizEngine.StartQuiz.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.QuizEngine.StartQuiz.CreateQuiz
{
    [ApiController]
    [Route("api/quizzes/{id}/start")]
    public class StartQuizEndPoint:ControllerBase
    {
        [HttpPost]

        public async Task<RequestResult<StartQuizResponseViewModel>> StartQuiz([FromBody] StartQuizRequestViewModel request, [FromServices] IMediator mediator, [FromServices] IMapper _mapper)
        {
            var result = await mediator.Send(new StartQuizOrchestrator(request.QuizId, request.StudentId));
            if (!result.IsSuccess)
                return RequestResult<StartQuizResponseViewModel>.Failure(result.ErrorCode, result.Message);

            return RequestResult<StartQuizResponseViewModel>.Success(_mapper.Map<StartQuizResponseViewModel>(result.Data, opt => { opt.Items["QuizId"] = request.QuizId; opt.Items["StudentId"] = request.StudentId; }));
        }
    }
}