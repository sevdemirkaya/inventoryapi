using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using InventoryApi.DataAccess; // AppDbContext burada tanımlıysa

namespace InventoryApi.DataAccess
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseSqlServer(
                "Server=localhost,5050;Database=InventoryDb;User Id=sa;Password=BimserDev2024!;TrustServerCertificate=True;");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}