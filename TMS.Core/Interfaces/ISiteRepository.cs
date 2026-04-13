using TMS.Core.Entities;

namespace TMS.Core.Interfaces;

public interface ISiteRepository : IRepository<Site>
{
    Task<IEnumerable<Site>> GetAllWithTerminalCountAsync();
    Task<IEnumerable<Site>> GetByWarehouseIdAsync(int warehouseId);
    Task<Site?> GetWithTerminalsAsync(int id);
}
