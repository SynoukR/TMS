using Microsoft.EntityFrameworkCore;
using TMS.Core.Entities;
using TMS.Core.Interfaces;
using TMS.Infrastructure.Data;

namespace TMS.Infrastructure.Repositories;

public class WarehouseRepository : Repository<Warehouse>, IWarehouseRepository
{
    public WarehouseRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Warehouse>> GetAllWithSitesAsync()
        => await _context.Warehouses
            .Include(w => w.Sites)
                .ThenInclude(s => s.Terminals)
            .Include(w => w.SpareTerminals)
            .OrderBy(w => w.Name)
            .ToListAsync();

    public async Task<Warehouse?> GetWithSitesAndTerminalsAsync(int id)
        => await _context.Warehouses
            .Include(w => w.Sites)
                .ThenInclude(s => s.Terminals)
            .Include(w => w.SpareTerminals)
            .FirstOrDefaultAsync(w => w.Id == id);
}
