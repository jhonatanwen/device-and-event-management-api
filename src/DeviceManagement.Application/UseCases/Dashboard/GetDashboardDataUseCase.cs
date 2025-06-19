using DeviceManagement.Application.Common;
using DeviceManagement.Domain.Enums;
using DeviceManagement.Domain.Repositories;

namespace DeviceManagement.Application.UseCases.Dashboard;

public sealed class GetDashboardDataUseCase(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<Result<DashboardResponse>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var endDate = DateTime.UtcNow;
            var startDate = endDate.AddDays(-7);

            var eventCounts = await _unitOfWork.Eventos.GetEventCountByTypeForPeriodAsync(startDate, endDate, cancellationToken);

            var eventCountResponses = new List<EventCountResponse>();

            foreach (var eventType in Enum.GetValues<EventType>())
            {
                var count = eventCounts.ContainsKey(eventType) ? eventCounts[eventType] : 0;
                eventCountResponses.Add(new EventCountResponse(eventType.ToString(), count));
            }

            var response = new DashboardResponse(
                startDate,
                endDate,
                eventCountResponses);

            return Result<DashboardResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<DashboardResponse>.Failure($"Erro interno: {ex.Message}");
        }
    }
}

public sealed record DashboardResponse(
    DateTime StartDate,
    DateTime EndDate,
    IEnumerable<EventCountResponse> EventCounts);

public sealed record EventCountResponse(
    string EventType,
    int Count);
