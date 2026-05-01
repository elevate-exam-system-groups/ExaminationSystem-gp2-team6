using ExaminationSystem.Common.Data;
using ExaminationSystem.Features.AdminManagement.Monitoring.Attempts.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.AdminManagement.Monitoring.Attempts
{
    [ApiController]
    [Route("api/admin/attempts")]
    [Authorize(Roles = "Admin")]
    public class AdminAttemptsEndPoint : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminAttemptsEndPoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAttempts(
            [FromQuery] Guid? studentId,
            [FromQuery] int? quizId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string sortBy = "submitted_at",
            [FromQuery] string order = "desc",
            CancellationToken cancellationToken = default)
        {
            var query = new GetAttemptsQuery(studentId, quizId, page, pageSize, sortBy, order);
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(new { result.Message });
            }

            return Ok(result.Data);
        }

        [HttpGet("{attemptId:int}")]
        public async Task<IActionResult> GetAttemptDetail(int attemptId, CancellationToken cancellationToken)
        {
            var query = new GetAttemptDetailQuery(attemptId);
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.IsSuccess)
            {
                if (result.ErrorCode == ErrorCode.NotFound)
                {
                    return NotFound(new { result.Message });
                }

                return BadRequest(new { result.Message });
            }

            return Ok(result.Data);
        }
    }
}
