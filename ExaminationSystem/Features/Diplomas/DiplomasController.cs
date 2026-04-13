using System.Security.Claims;
using ExaminationSystem.Common.Views;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Diplomas;

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
}
