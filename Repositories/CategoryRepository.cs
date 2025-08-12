using InventoryApi.Model.DTOs;              // Filter burada
using InventoryApi.Model.Entities;
using InventoryApi.Model.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.DataAccess.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;
    public CategoryRepository(AppDbContext context) => _context = context;

    public async Task<List<Category>> GetAllAsync()
    {
        return await _context.Categories
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(string id)
    {
        // Id PK ve string ise FindAsync gayet uygun;
        // değilse FirstOrDefaultAsync kullanın.
        var entity = await _context.Categories.FindAsync(id);
        return entity; // AsNoTracking gerekmez; Find change tracker'a bağlı
    }

    public async Task<Category> AddAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var entity = await _context.Categories.FindAsync(id);
        if (entity is null) return;

        _context.Categories.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Category>> SearchByNameAsync(string name)
    {
        // Case-insensitive ihtiyacınız varsa veritabanı kolasyonu ile çözülür.
        return await _context.Categories
            .AsNoTracking()
            .Where(c => EF.Functions.Like(c.Name, $"%{name}%"))
            .ToListAsync();
    }

    public async Task<List<Category>> Search(Filter filter)
    {
        IQueryable<Category> q = _context.Categories.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Name))
            q = q.Where(c => EF.Functions.Like(c.Name, $"%{filter.Name}%"));

        // Filter genişleyecekse (CreatedAt, UpdatedAt vs.) burada ekleyin.

        return await q.ToListAsync();
    }
}
