using TMS.Core.Enums;

namespace TMS.API.DTOs;

public record TerminalSummaryDto(
    int Id,
    string Name,
    string SerialNumber,
    string Model,
    string? IpAddress,
    TerminalStatus Status,
    EquipmentStatus EquipmentStatus,
    DateTime? LastSeen
);
