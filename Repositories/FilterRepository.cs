using InventoryApi.Model.Entities;
using InventoryApi.Model.Interfaces;

namespace InventoryApi.DataAccess.Repositories;

public class FilterRepository : IFilterRepository
{
    private readonly AppDbContext _context;

    public FilterRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<IEnumerable<Filter>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Filter?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    Task IFilterRepository.
    {
        throw new NotImplementedException();
    }

    public Task<Filter> UpdateAsync(Filter filter)
    {
        throw new NotImplementedException();
    }

    public Task<Filter> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    Filter IFilterRepository.
    {
        throw new NotImplementedException();
    }
}