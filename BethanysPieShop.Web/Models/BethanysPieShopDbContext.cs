using Microsoft.EntityFrameworkCore;

namespace BethanysPieShop.Web.Models;

public class BethanysPieShopDbContext : DbContext
{
    public BethanysPieShopDbContext(DbContextOptions<BethanysPieShopDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Pie> Pies { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
}