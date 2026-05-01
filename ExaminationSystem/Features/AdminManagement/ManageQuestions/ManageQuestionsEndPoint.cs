using ExaminationSystem.Common.Data;
using ExaminationSystem.Features.AdminManagement.ManageQuestions.Commands;
using ExaminationSystem.Features.AdminManagement.ManageQuestions.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.AdminManagement.ManageQuestions
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class ManageQuestionsEndPoint : ControllerBase
    {
        private readonly IMediator _mediator;

        public ManageQuestionsEndPoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("quizzes/{quiz_id:int}/questions")]
        public async Task<IActionResult> AddQuestion(int quiz_id, [FromBody] AddQuestionRequest body, CancellationToken cancellationToken)
        {
            var command = new AddQuestionCommand(quiz_id, body.Text, body.Options, body.Explanation);
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return result.ErrorCode switch
                {
                    ErrorCode.NotFound => NotFound(new { result.Message }),
                    ErrorCode.ValidationError => UnprocessableEntity(new { result.Message }),
                    ErrorCode.Conflict => Conflict(new { result.Message }),
                    _ => BadRequest(new { result.Message })
                };
            }

            return CreatedAtAction(nameof(AddQuestion), new { quiz_id }, result);
        }

        [HttpPut("questions/{id:int}")]
        public async Task<IActionResult> UpdateQuestion(int id, [FromBody] UpdateQuestionRequest body, CancellationToken cancellationToken)
        {
            var command = new UpdateQuestionCommand(id, body.Text, body.Options, body.Explanation);
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return result.ErrorCode switch
                {
                    ErrorCode.NotFound => NotFound(new { result.Message }),
                    ErrorCode.ValidationError => UnprocessableEntity(new { result.Message }),
                    ErrorCode.Conflict => Conflict(new { result.Message }),
                    _ => BadRequest(new { result.Message })
                };
            }

            return Ok(result);
        }

        [HttpDelete("questions/{id:int}")]
        public async Task<IActionResult> DeleteQuestion(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteQuestionCommand(id);
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return result.ErrorCode switch
                {
                    ErrorCode.NotFound => NotFound(new { result.Message }),
                    ErrorCode.Conflict => Conflict(new { result.Message }),
                    _ => BadRequest(new { result.Message })
                };
            }

            return NoContent();
        }
    }

    public record AddQuestionRequest(string Text, List<OptionDto> Options, string? Explanation);
    public record UpdateQuestionRequest(string Text, List<OptionDto> Options, string? Explanation);
}
