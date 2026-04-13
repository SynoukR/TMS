namespace TMS.API.DTOs;

public record SiteDto(
    int Id,
    string Name,
    string? Address,
    bool IsActive,
    int WarehouseId,
    string WarehouseName,
    int TerminalCount,
    DateTime CreatedAt
);

public record SiteDetailDto(
    int Id,
    string Name,
    string? Address,
    bool IsActive,
    int WarehouseId,
    string WarehouseName,
    DateTime CreatedAt,
    IEnumerable<TerminalSummaryDto> Terminals
);

public record CreateSiteDto(string Name, string? Address, bool IsActive, int WarehouseId);
