using System.Security.Claims;
using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.StudentDashboard.ViewDiplomas.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.StudentDashboard.ViewDiplomas;

[ApiController]
[Route("api/diplomas")]
[Authorize(Roles = "student,Student")]
public sealed class DiplomasController : ControllerBase
{
    private readonly IMediator _mediator;

    public DiplomasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(EndpointResponse<GetPublishedDiplomasResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<EndpointResponse<GetPublishedDiplomasResponseDto>>> GetPublishedDiplomas(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var studentId))
        {
            return Unauthorized();
        }

        var result = await _mediator.Send(
            new GetPublishedDiplomasQuery(studentId, pageNumber, pageSize),
            cancellationToken);

        return Ok(EndpointResponse<GetPublishedDiplomasResponseDto>.Success(result.Data));
    }

    [HttpGet("{diplomaId:int}/quizzes")]
    [ProducesResponseType(typeof(EndpointResponse<IReadOnlyList<DiplomaQuizItemDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EndpointResponse<IReadOnlyList<DiplomaQuizItemDto>>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(EndpointResponse<IReadOnlyList<DiplomaQuizItemDto>>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<EndpointResponse<IReadOnlyList<DiplomaQuizItemDto>>>> GetDiplomaQuizzes(
        [FromRoute] int diplomaId,
        CancellationToken cancellationToken = default)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var studentId))
        {
            return Unauthorized();
        }

        var result = await _mediator.Send(new GetDiplomaQuizzesQuery(diplomaId, studentId), cancellationToken);

        if (!result.IsSuccess)
        {
            if (result.ErrorCode == ErrorCode.NotFound)
                return NotFound(EndpointResponse<IReadOnlyList<DiplomaQuizItemDto>>.Failure(result.ErrorCode, result.Message));

            if (result.ErrorCode == ErrorCode.Unauthorized)
                return StatusCode(StatusCodes.Status403Forbidden, EndpointResponse<IReadOnlyList<DiplomaQuizItemDto>>.Failure(result.ErrorCode, result.Message));
        }

        return Ok(EndpointResponse<IReadOnlyList<DiplomaQuizItemDto>>.Success(result.Data));
    }
}
