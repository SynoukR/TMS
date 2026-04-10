using Microsoft.EntityFrameworkCore;
using TMS.Core.Entities;
using TMS.Core.Enums;
using TMS.Core.Interfaces;
using TMS.Infrastructure.Data;

namespace TMS.Infrastructure.Repositories;

public class CommandRepository : Repository<Command>, ICommandRepository
{
    public CommandRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Command>> GetByTerminalIdAsync(int terminalId) =>
        await _dbSet.Where(c => c.TerminalId == terminalId)
                    .Include(c => c.Logs)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToListAsync();

    public async Task<IEnumerable<Command>> GetPendingCommandsAsync() =>
        await _dbSet.Where(c => c.Status == CommandStatus.Pending)
                    .Include(c => c.Terminal)
                    .ToListAsync();
}
