using InventoryApi.Model.Entities;
using InventoryApi.Model.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.DataAccess.Repositories;

public class StockMovementRepository : IStockMovementRepository
{
    private readonly AppDbContext _context;
    public StockMovementRepository(AppDbContext context) => _context = context;

    public async Task<List<StockMovement>> GetAllAsync() =>
        await _context.StockMovements.AsNoTracking().ToListAsync();

   public async Task<StockMovement?> GetByIdAsync(string id)
{
    return await _context.StockMovements
        .AsNoTracking()
        .FirstOrDefaultAsync(sm => sm.Id == id);
}


    public async Task AddAsync(StockMovement movement)
    {
        movement.Id = Guid.NewGuid().ToString();
        _context.StockMovements.Add(movement);
        await _context.SaveChangesAsync();
    }

    public async Task<List<StockMovement>> GetByProductIdAsync(string productId)
    {
        if (string.IsNullOrWhiteSpace(productId))
            return new List<StockMovement>();

        return await _context.StockMovements
            .AsNoTracking()
            .Where(sm => sm.ProductId == productId)
            .ToListAsync();
    }

    public async Task UpdateAsync(StockMovement movement)
    {
        _context.StockMovements.Update(movement);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var movement = await _context.StockMovements.FirstOrDefaultAsync(x => x.Id == id);
        if (movement is null) return;
        _context.StockMovements.Remove(movement);
        await _context.SaveChangesAsync();
    }

}
