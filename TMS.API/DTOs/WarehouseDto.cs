namespace TMS.API.DTOs;

public record WarehouseDto(
    int Id,
    string Name,
    string? Address,
    bool IsActive,
    int SiteCount,
    int SpareCount,
    DateTime CreatedAt
);

public record WarehouseDetailDto(
    int Id,
    string Name,
    string? Address,
    bool IsActive,
    DateTime CreatedAt,
    IEnumerable<SiteDto> Sites,
    IEnumerable<TerminalSummaryDto> SpareTerminals
);

public record CreateWarehouseDto(string Name, string? Address, bool IsActive);
