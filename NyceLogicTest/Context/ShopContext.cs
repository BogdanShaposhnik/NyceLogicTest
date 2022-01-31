using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using NyceLogicTest.Models;

namespace NyceLogicTest.Context
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Item> Item { get; set; }
        public DbSet<ShoppingList> ShoppingList { get; set; }
        public DbSet<ItemToShoppingList> ItemToShoppingList { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
