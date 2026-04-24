using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.AdminManagement.Monitoring.Analytics.Dtos;
using ExaminationSystem.Features.AdminManagement.Monitoring.Analytics.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.AdminManagement.Monitoring.Analytics
{
    [ApiController]
    [Route("api/admin/analytics")]
    [Authorize(Roles = "Admin")]
    public class AnalyticsEndPoint : ControllerBase
    {
        private readonly IMediator _mediator;

        public AnalyticsEndPoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(EndpointResponse<AnalyticsDashboardDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAnalytics(
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] int? diplomaId,
            CancellationToken cancellationToken)
        {
            var query = new GetPerformanceAnalyticsQuery(from, to, diplomaId);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(EndpointResponse<AnalyticsDashboardDto>.Success(result.Data));
        }
    }
}
