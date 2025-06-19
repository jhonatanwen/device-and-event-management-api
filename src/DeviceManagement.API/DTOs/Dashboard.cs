namespace DeviceManagement.API.DTOs;

public sealed record Dashboard(
    DateTime StartDate,
    DateTime EndDate,
    IEnumerable<EventCount> EventCounts);

public sealed record EventCount(
    string EventType,
    int Count);
