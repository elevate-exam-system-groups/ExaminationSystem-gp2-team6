using AutoMapper;
using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.AdminManagement.UpdateQuiz.Orchestrator;
using ExaminationSystem.Features.AdminManagement.UpdateQuiz.ViewModel;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.AdminManagement.UpdateQuiz
{
    [ApiController]
    [Route("api/admin/quizzes")]
    public class UpdateQuizEndPoint : ControllerBase
    {
        private readonly IValidator<UpdateQuizRequestViewModel> _validator;

        public UpdateQuizEndPoint(IValidator<UpdateQuizRequestViewModel> validator, IMapper mapper)
        {
            _validator = validator;
        }
        [HttpPut("{id}")]
        public async Task<EndpointResponse<bool>> UpdateQuiz([FromRoute] int id, [FromBody] UpdateQuizRequestViewModel request, [FromServices] IMediator mediator)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return EndpointResponse<bool>.Failure(ErrorCode.InvalidData, string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

            var quizUpdated = await mediator.Send(new UpdateQuizOrchestrator(id, request.Title, request.DiplomaId, request.DurationMinutes, request.PassScore, request.MaxAttempts, request.Instructions));

            return (!quizUpdated.IsSuccess)
                ? EndpointResponse<bool>.Failure(quizUpdated.ErrorCode, quizUpdated.Message)
                : EndpointResponse<bool>.Success(quizUpdated.Data, quizUpdated.Message);

        }
    }
}
