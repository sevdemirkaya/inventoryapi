using InventoryApi.Model.Entities;
using InventoryApi.Model.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.DataAccess.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    public ProductRepository(AppDbContext context) => _context = context;

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }
    public async Task<Product> GetByIdAsync(string id)
    {
        return await _context.Products.FindAsync(id);
    }
    public async Task AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        Product product = await GetByIdAsync(id);
        if (product is null) return;
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Product>> GetProductsByNameAndCategoryId(string productName, string categoryId)
    {
        
        return await _context.Products
            .AsNoTracking()
            .Where(p => p.CategoryId == categoryId && p.Name.Contains(productName))
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<List<Product>> GetProductsByName(string filterName)
    {
        return await _context.Products
            .Where(p => !string.IsNullOrWhiteSpace(p.Name) &&
                        p.Name.Contains(filterName))
            .ToListAsync();
    }

    public async Task<List<Product>> GetProductsByCategoryId(string filterCategoryId)
    {
        return await _context.Products
            .Where(p => p.CategoryId.ToString() == filterCategoryId)
            .ToListAsync();
    }
    
    
}