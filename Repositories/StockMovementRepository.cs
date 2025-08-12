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
    
    public async Task<List<StockMovement>> GetByProductIdAsync(string productId)
    {
        if (string.IsNullOrWhiteSpace(productId))
            return new List<StockMovement>();

        return await _context.StockMovements
            .Where(sm => sm.ProductId == productId)
            .ToListAsync();
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

    public async Task<List<StockMovement>> Search(Filter filter)
{
    IQueryable<StockMovement> q = _context.StockMovements.AsNoTracking();

    // 1) İsim (opsiyonel)
    if (!string.IsNullOrWhiteSpace(filter.Name))
        q = q.Where(sm => EF.Functions.Like(sm.Name, $"%{filter.Name}%"));

    // 2) CategoryId (Filter.CategoryId string; entity int) 
    if (!string.IsNullOrWhiteSpace(filter.CategoryId) && int.TryParse(filter.CategoryId, out var catId))
        q = q.Where(sm => sm.CategoryId == catId);

    // 3) Tarih aralığı (EndDate dahil olacak şekilde)
    if (filter.StartDate.HasValue)
        q = q.Where(sm => sm.Date >= filter.StartDate.Value);

    if (filter.EndDate.HasValue)
    {
        var endExclusive = filter.EndDate.Value.Date.AddDays(1);
        q = q.Where(sm => sm.Date < endExclusive);
    }

    // 4) Generic Column/Value filtresi
    if (!string.IsNullOrWhiteSpace(filter.Column) && !string.IsNullOrWhiteSpace(filter.Value))
    {
        switch (filter.Column.Trim().ToLowerInvariant())
        {
            case "productid":
                q = q.Where(sm => sm.ProductId == filter.Value);
                break;

            case "movementtype":
                if (TryParseMovementType(filter.Value, out var mt))
                    q = q.Where(sm => sm.MovementType == mt);
                break;

            case "quantity":
                if (int.TryParse(filter.Value, out var qty))
                    q = q.Where(sm => sm.Quantity == qty);
                break;

            case "name":
                q = q.Where(sm => EF.Functions.Like(sm.Name, $"%{filter.Value}%"));
                break;

            case "categoryid":
                if (int.TryParse(filter.Value, out var cid))
                    q = q.Where(sm => sm.CategoryId == cid);
                break;

            // gerekirse ek kolonlar: "date", "datefrom", "dateto" vs.
        }
    }

    return await q.ToListAsync();

    // local helper
    static bool TryParseMovementType(string value, out MovementType mt)
    {
        // "in"/"out" veya "1"/"0" destekle
        mt = MovementType.In;
        if (string.IsNullOrWhiteSpace(value)) return false;

        var v = value.Trim();
        if (int.TryParse(v, out var n))
        {
            if (n == 1) { mt = MovementType.In;  return true; }
            if (n == 0) { mt = MovementType.Out; return true; }
            return false;
        }

        if (v.Equals("in", StringComparison.OrdinalIgnoreCase))  { mt = MovementType.In;  return true; }
        if (v.Equals("out", StringComparison.OrdinalIgnoreCase)) { mt = MovementType.Out; return true; }
        return false;
    }
}

}