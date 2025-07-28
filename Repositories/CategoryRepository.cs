using InventoryApi.Model.DTOs;
using InventoryApi.Model.Entities;
using InventoryApi.Model.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.DataAccess.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public Task<Category?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task<Category> AddAsync(Category category)
    {
        _context.Categories.Add(category); 
        await _context.SaveChangesAsync();
        return category;
    }

    public Task UpdateAsync(CategoryDTO category)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category is null) return;
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Category>> SearchByNameAsync(string name)
    {
        return await _context.Categories
            .Where(c => c.Name.Contains(name))
            .ToListAsync();
    }
}