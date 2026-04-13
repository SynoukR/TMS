namespace TMS.Core.Entities;

public class Site
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;
    public ICollection<Terminal> Terminals { get; set; } = [];
}
