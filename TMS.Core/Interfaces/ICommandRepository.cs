using TMS.Core.Entities;
using TMS.Core.Enums;

namespace TMS.Core.Interfaces;

public interface ICommandRepository : IRepository<Command>
{
    Task<IEnumerable<Command>> GetByTerminalIdAsync(int terminalId);
    Task<IEnumerable<Command>> GetPendingCommandsAsync();
}
