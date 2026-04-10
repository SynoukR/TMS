using TMS.Core.Entities;
using TMS.Core.Enums;

namespace TMS.Core.Interfaces;

public interface ITerminalRepository : IRepository<Terminal>
{
    Task<Terminal?> GetBySerialNumberAsync(string serialNumber);
    Task<IEnumerable<Terminal>> GetByStatusAsync(TerminalStatus status);
}
