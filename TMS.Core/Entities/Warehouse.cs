namespace TMS.Core.Entities;

public class Warehouse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Site> Sites { get; set; } = [];
    public ICollection<Terminal> SpareTerminals { get; set; } = [];
}
