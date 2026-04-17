using AutoMapper;
using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.AdminManagement.CreateQuiz.ViewModel;
using ExaminationSystem.Features.QuizEngine.StartQuiz.Orchestrator;
using ExaminationSystem.Features.QuizEngine.StartQuiz.ViewModel;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.QuizEngine.StartQuiz.CreateQuiz
{
    [ApiController]
    [Route("api/quizzes/{id}/start")]
    public class StartQuizEndPoint:ControllerBase
    {
        private readonly IValidator<StartQuizRequestViewModel> _validator;

        public StartQuizEndPoint(IValidator<StartQuizRequestViewModel> validator)
        {
            _validator = validator;
        }
        [HttpPost]
        public async Task<RequestResult<StartQuizResponseViewModel>> StartQuiz([FromBody] StartQuizRequestViewModel request, [FromServices] IMediator mediator, [FromServices] IMapper _mapper)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return RequestResult<StartQuizResponseViewModel>.Failure(ErrorCode.InvalidData, string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

            var result = await mediator.Send(new StartQuizOrchestrator(request.QuizId, request.StudentId));
            if (!result.IsSuccess)
                return RequestResult<StartQuizResponseViewModel>.Failure(result.ErrorCode, result.Message);

            return RequestResult<StartQuizResponseViewModel>.Success(_mapper.Map<StartQuizResponseViewModel>(result.Data, opt => { opt.Items["QuizId"] = request.QuizId; opt.Items["StudentId"] = request.StudentId; }));
        }
    }
}