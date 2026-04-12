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
        private readonly IMediator mediator;
        IValidator<CreateQuizRequestViewModel> validator;
        public CreateQuizEndPoint(IMediator _mediator,IValidator<CreateQuizRequestViewModel> _validator)
        {
            mediator = _mediator;
            validator = _validator;
        }

        [HttpPost]
        public async Task<EndpointResponse<CreateQuizResponseViewModel>> CreateQuiz([FromBody] CreateQuizRequestViewModel request)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid) { 
                return EndpointResponse<CreateQuizResponseViewModel>.Failure(ErrorCode.InvalidData, string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }
            //var quizId = await mediator.Send(command);
            throw new NotImplementedException();
            //return Ok(quizId);
        }
    }
}
