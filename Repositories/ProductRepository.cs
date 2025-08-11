using InventoryApi.Model.Entities;
using InventoryApi.Model.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.DataAccess.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    public ProductRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Product>> GetAllAsync() => await _context.Products.ToListAsync();
    public async Task<Product?> GetByIdAsync(string id) => await _context.Products.FindAsync(id);
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
        var product = await _context.Products.FindAsync(id);
        if (product is null) return;
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Product>> GetProductByNameAndCategoryId(string productName, string categoryId)
    {
        if (string.IsNullOrWhiteSpace(productName) || !int.TryParse(categoryId, out var catId))
            return new List<Product>();

        var name = productName.Trim();

        return await _context.Products
            .AsNoTracking()
            .Where(p => p.CategoryId == catId
                        && !string.IsNullOrEmpty(p.Name)
                        && EF.Functions.Like(p.Name, $"%{name}%"))
            .OrderBy(p => p.Name)
            .ToListAsync();
        
    }

    public async Task<IEnumerable<Product>> GetProductByName(string filterName)
    {
        return await _context.Products
            .Where(p => !string.IsNullOrEmpty(p.Name) &&
                        p.Name.Contains(filterName))
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByCategoryId(string filterCategoryId)
    {
        return await _context.Products
            .Where(p => p.CategoryId.ToString() == filterCategoryId)
            .ToListAsync();
    }


    public bool SearchInName(string name, string filterName)
    {
        return name.Contains(filterName);
    }
    


    
}