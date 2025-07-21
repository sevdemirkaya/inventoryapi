using InventoryApi.Model.Entities;
using InventoryApi.Model.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.DataAccess.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;
    public async Task<Category?> GetByIdAsync(Guid id) => await _context.Categories.FindAsync(id);


    public async Task<List<Category>> GetAllAsync() => await _context.Categories.ToListAsync();

    public async Task<Category?> GetByIdAsync(int id) => await _context.Categories.FindAsync(id);

    public async Task AddAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    public Task UpdateAsync(Category category)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category is null) return;
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }
}