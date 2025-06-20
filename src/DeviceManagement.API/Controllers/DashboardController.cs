using DeviceManagement.API.Contracts;
using DeviceManagement.Application.UseCases.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagement.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public sealed class DashboardController(GetDashboardDataUseCase getDashboardDataUseCase) : ControllerBase
{
    private readonly GetDashboardDataUseCase _getDashboardDataUseCase = getDashboardDataUseCase;

  [HttpGet]
    public async Task<ActionResult<Dashboard>> Get(CancellationToken cancellationToken)
    {
        var result = await _getDashboardDataUseCase.ExecuteAsync(cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        var eventCounts = result.Data!.EventCounts.Select(ec =>
            new EventCount(ec.EventType, ec.Count));

        var dashboard = new Dashboard(
            result.Data.StartDate,
            result.Data.EndDate,
            eventCounts);

        return Ok(dashboard);
    }
}
