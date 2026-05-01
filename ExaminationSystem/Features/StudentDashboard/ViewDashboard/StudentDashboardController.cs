using System.Security.Claims;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.StudentDashboard.ViewDashboard.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.StudentDashboard.ViewDashboard;

[ApiController]
[Route("api/student")]
[Authorize(Roles = "Student")]
public class StudentDashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public StudentDashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("dashboard")]
    [ProducesResponseType(typeof(EndpointResponse<StudentDashboardResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<EndpointResponse<StudentDashboardResponseDto>>> GetDashboard(CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var studentId))
            return Unauthorized();

        var result = await _mediator.Send(new GetStudentDashboardQuery(studentId), cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(EndpointResponse<StudentDashboardResponseDto>.Failure(result.ErrorCode, result.Message));

        return Ok(EndpointResponse<StudentDashboardResponseDto>.Success(result.Data));
    }
}
