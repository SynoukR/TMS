using TMS.Core.Enums;

namespace TMS.Core.Entities;

public class Terminal
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string? IpAddress { get; set; }
    public string? Location { get; set; }
    public TerminalStatus Status { get; set; } = TerminalStatus.Unknown;
    public DateTime? LastSeen { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Command> Commands { get; set; } = new List<Command>();
}
