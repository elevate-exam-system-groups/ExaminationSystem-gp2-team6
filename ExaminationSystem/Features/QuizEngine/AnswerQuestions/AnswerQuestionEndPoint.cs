using AutoMapper;
using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.QuizEngine.AnswerQuestions.Orchestrator;
using ExaminationSystem.Features.QuizEngine.AnswerQuestions.ViewModels;
using ExaminationSystem.Features.QuizEngine.StartQuiz.ViewModel;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.QuizEngine.AnswerQuestions
{
    public class AnswerQuestionEndPoint:ControllerBase
    {
        private readonly IValidator<AnswerQuestionRequestViewModel> _validator;
        public AnswerQuestionEndPoint(IValidator<AnswerQuestionRequestViewModel> validator)
        {
            _validator = validator;
        }

        [HttpPost("api/quiz/answer")]
        public async Task<EndpointResponse<bool>> AnswerQuestion([FromBody] AnswerQuestionRequestViewModel request, CancellationToken cancellationToken, [FromServices] IMediator mediator, [FromServices] IMapper mapper)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return EndpointResponse<bool>.Failure(ErrorCode.ValidationError, validationResult.Errors.First().ErrorMessage);
            

            var result =await mediator.Send(new AnswerQuestionOrchestrator(request.AttemptId, request.QuestionId, request.OptionId, request.StudentId), cancellationToken);
            return result.IsSuccess 
                ? EndpointResponse<bool>.Success(true) 
                : EndpointResponse<bool>.Failure(result.ErrorCode, result.Message);
        }
    }
}
