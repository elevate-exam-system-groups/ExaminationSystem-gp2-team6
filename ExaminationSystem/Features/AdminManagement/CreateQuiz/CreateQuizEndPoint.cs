using AutoMapper;
using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.AdminManagement.UpdateQuiz.Commands;
using ExaminationSystem.Features.AdminManagement.CreateQuiz.ViewModel;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ExaminationSystem.Features.AdminManagement.CreateQuiz.Orchestrator;
using ExaminationSystem.Features.AdminManagement.UpdateQuiz.Orchestrator;

namespace ExaminationSystem.Features.AdminManagement.CreateQuiz
{
    [ApiController]
    [Route("api/admin/quizzes")]
    public class CreateQuizEndPoint : ControllerBase
    {
        private readonly IValidator<CreateQuizRequestViewModel> _validator;
        private readonly IMapper _mapper;
        
        public CreateQuizEndPoint(IValidator<CreateQuizRequestViewModel> validator, IMapper mapper)
        {
            _validator = validator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<EndpointResponse<bool>> CreateQuiz([FromBody] CreateQuizRequestViewModel request, [FromServices] IMediator mediator)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid) 
                return EndpointResponse<bool>.Failure(ErrorCode.InvalidData, string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));
            
            var quizCreated = await mediator.Send(new CreateQuizOrchestrator(request.Title, request.DiplomaId, request.DurationMinutes, request.PassScore, request.MaxAttempts, request.Instructions));
            
            return (!quizCreated.IsSuccess)
                ? EndpointResponse<bool>.Failure(quizCreated.ErrorCode, quizCreated.Message)
                : EndpointResponse<bool>.Success(quizCreated.Data, quizCreated.Message);
        }
    }
}
