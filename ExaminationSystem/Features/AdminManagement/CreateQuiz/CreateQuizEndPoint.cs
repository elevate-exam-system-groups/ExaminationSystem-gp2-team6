using AutoMapper;
using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.AdminManagement.CreateQuiz.Commands;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.AdminManagement.CreateQuiz
{
    [ApiController]
    [Route("api/[controller]")]
    public class CreateQuizEndPoint:ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<CreateQuizRequestViewModel> _validator;
        private readonly IMapper _mapper;
        public CreateQuizEndPoint(IMediator mediator,IValidator<CreateQuizRequestViewModel> validator, IMapper mapper)
        {
            _mediator = mediator;
            _validator = validator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<EndpointResponse<CreateQuizResponseViewModel>> CreateQuiz([FromBody] CreateQuizRequestViewModel request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid) { 
                return EndpointResponse<CreateQuizResponseViewModel>.Failure(ErrorCode.InvalidData, string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            var quizCreated =await _mediator.Send(new CreateQuizCommand(request.Title,request.Duration,request.PassScore,request.MaxAttempts,request.Status,request.Instructions));
            return quizCreated.IsSuccess 
                ? EndpointResponse<CreateQuizResponseViewModel>.Success(_mapper.Map<CreateQuizResponseViewModel>(quizCreated.Data), "Quiz created successfully") 
                : EndpointResponse<CreateQuizResponseViewModel>.Failure(quizCreated.ErrorCode, quizCreated.Message);
        }
    }
}
