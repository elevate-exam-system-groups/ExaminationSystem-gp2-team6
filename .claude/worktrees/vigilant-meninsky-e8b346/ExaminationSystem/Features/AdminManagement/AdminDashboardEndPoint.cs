using ExaminationSystem.Features.AdminManagement.AdminDashboard.Queries.GetAdminStats;
using ExaminationSystem.Features.AdminManagement.Diplomas.Commends;
using ExaminationSystem.Features.AdminManagement.Diplomas.Commends.DeleteDiploma;
using ExaminationSystem.Features.AdminManagement.Diplomas.Commends.UpdateDiploma;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.AdminManagement
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminDashboardEndPoint(IMediator mediator) : ControllerBase
    {
        //Dasboard
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats(CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetAdminStatsQuery(), cancellationToken);
            return Ok(result);
        }

        //CreateDiploma
        [HttpPost("diplomas")]
        public async Task<IActionResult> CreateDiploma(
            [FromBody] CreateDiplomaCommand command,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(CreateDiploma), new { id = result.Id }, result);
        }

        //Update Diploma 
        [HttpPut("diplomas/{id:int}")]
        public async Task<IActionResult> UpdateDiploma(
            int id,
            [FromBody] UpdateDiplomaRequest body,
            CancellationToken cancellationToken)
        {
            var command = new UpdateDiplomaCommand(id, body.Title, body.Description);
            var result = await mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        // Delete Diploma (Soft)
        [HttpDelete("diplomas/{id:int}")]
        public async Task<IActionResult> DeleteDiploma(
            int id, CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteDiplomaCommand(id), cancellationToken);
            return NoContent();
        }
    }
    public record UpdateDiplomaRequest(string Title, string? Description);
}
