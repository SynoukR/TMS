namespace TMS.Core.Entities;

public class CommandLog
{
    public int Id { get; set; }
    public int CommandId { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public Command Command { get; set; } = null!;
}
