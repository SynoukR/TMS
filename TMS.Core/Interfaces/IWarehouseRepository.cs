using TMS.Core.Entities;

namespace TMS.Core.Interfaces;

public interface IWarehouseRepository : IRepository<Warehouse>
{
    Task<IEnumerable<Warehouse>> GetAllWithSitesAsync();
    Task<Warehouse?> GetWithSitesAndTerminalsAsync(int id);
}
