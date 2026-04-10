using TMS.Core.Enums;

namespace TMS.Core.Entities;

public class Command
{
    public int Id { get; set; }
    public int TerminalId { get; set; }
    public CommandType Type { get; set; }
    public string? Payload { get; set; }
    public CommandStatus Status { get; set; } = CommandStatus.Pending;
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? SentAt { get; set; }
    public DateTime? ExecutedAt { get; set; }

    public Terminal Terminal { get; set; } = null!;
    public ICollection<CommandLog> Logs { get; set; } = new List<CommandLog>();
}
