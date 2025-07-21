using InventoryApi.Model.Entities;
using InventoryApi.Model.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.DataAccess.Repositories;

public class StockMovementRepository : IStockMovementRepository
{
    private readonly AppDbContext _context;
    public StockMovementRepository(AppDbContext context) => _context = context;

    public async Task<List<StockMovement>> GetAllAsync() => await _context.StockMovements.ToListAsync();
    public async Task<StockMovement?> GetByIdAsync(int id) => await _context.StockMovements.FindAsync(id);
    public async Task AddAsync(StockMovement movement)
    {
        _context.StockMovements.Add(movement);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(StockMovement movement)
    {
        _context.StockMovements.Update(movement);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var movement = await _context.StockMovements.FindAsync(id);
        if (movement is null) return;
        _context.StockMovements.Remove(movement);
        await _context.SaveChangesAsync();
    }
}