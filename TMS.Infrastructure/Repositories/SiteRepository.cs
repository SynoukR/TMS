using Microsoft.EntityFrameworkCore;
using TMS.Core.Entities;
using TMS.Core.Interfaces;
using TMS.Infrastructure.Data;

namespace TMS.Infrastructure.Repositories;

public class SiteRepository : Repository<Site>, ISiteRepository
{
    public SiteRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Site>> GetAllWithTerminalCountAsync()
        => await _context.Sites
            .Include(s => s.Terminals)
            .Include(s => s.Warehouse)
            .OrderBy(s => s.Name)
            .ToListAsync();

    public async Task<IEnumerable<Site>> GetByWarehouseIdAsync(int warehouseId)
        => await _context.Sites
            .Where(s => s.WarehouseId == warehouseId)
            .Include(s => s.Terminals)
            .OrderBy(s => s.Name)
            .ToListAsync();

    public async Task<Site?> GetWithTerminalsAsync(int id)
        => await _context.Sites
            .Include(s => s.Terminals)
            .Include(s => s.Warehouse)
            .FirstOrDefaultAsync(s => s.Id == id);
}
