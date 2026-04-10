using Microsoft.EntityFrameworkCore;
using TMS.Core.Entities;
using TMS.Core.Enums;
using TMS.Core.Interfaces;
using TMS.Infrastructure.Data;

namespace TMS.Infrastructure.Repositories;

public class TerminalRepository : Repository<Terminal>, ITerminalRepository
{
    public TerminalRepository(AppDbContext context) : base(context) { }

    public async Task<Terminal?> GetBySerialNumberAsync(string serialNumber) =>
        await _dbSet.FirstOrDefaultAsync(t => t.SerialNumber == serialNumber);

    public async Task<IEnumerable<Terminal>> GetByStatusAsync(TerminalStatus status) =>
        await _dbSet.Where(t => t.Status == status).ToListAsync();
}
