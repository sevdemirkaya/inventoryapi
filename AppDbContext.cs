namespace InventoryApi.DataAccess;
using Microsoft.EntityFrameworkCore;
using InventoryApi.Model.Entities;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
   
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();
    public DbSet<Filter> Filters => Set<Filter>();

        
   
}
