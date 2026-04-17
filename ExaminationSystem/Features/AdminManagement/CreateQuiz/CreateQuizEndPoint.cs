using AutoMapper;
using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.AdminManagement.CreateQuiz.Commands;
using ExaminationSystem.Features.AdminManagement.CreateQuiz.ViewModel;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.AdminManagement.CreateQuiz
{
    [ApiController]
    [Route("api/[controller]")]
    public class CreateQuizEndPoint:ControllerBase
    {
        private readonly IValidator<CreateQuizRequestViewModel> _validator;
        public CreateQuizEndPoint(IValidator<CreateQuizRequestViewModel> validator)
        {
            _validator = validator;
        }

        [HttpPost]
        public async Task<EndpointResponse<CreateQuizResponseViewModel>> CreateQuiz([FromBody] CreateQuizRequestViewModel request, [FromServices] IMediator mediator, [FromServices] IMapper mapper)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid) { 
                return EndpointResponse<CreateQuizResponseViewModel>.Failure(ErrorCode.InvalidData, string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            var quizCreated =await mediator.Send(new CreateQuizCommand(request.Title,request.Duration,request.PassScore,request.MaxAttempts,request.Status,request.Instructions));
            return quizCreated.IsSuccess 
                ? EndpointResponse<CreateQuizResponseViewModel>.Success(mapper.Map<CreateQuizResponseViewModel>(quizCreated.Data), "Quiz created successfully") 
                : EndpointResponse<CreateQuizResponseViewModel>.Failure(quizCreated.ErrorCode, quizCreated.Message);
        }
    }
}
