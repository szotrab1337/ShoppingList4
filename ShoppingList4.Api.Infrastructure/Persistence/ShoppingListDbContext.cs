using Microsoft.EntityFrameworkCore;
using ShoppingList4.Api.Domain.Entities;

namespace ShoppingList4.Api.Infrastructure.Persistence
{
    public class ShoppingListDbContext(DbContextOptions<ShoppingListDbContext> options) : DbContext(options)
    {
        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingList>()
                .Property(x => x.Name)
                .HasMaxLength(35);

            modelBuilder.Entity<ShoppingList>()
                .Property(x => x.CreatedOn)
                .HasDefaultValueSql("GetDate()");

            modelBuilder.Entity<ShoppingList>()
                .HasMany(x => x.Entries)
                .WithOne(z => z.ShoppingList)
                .HasForeignKey(z => z.ShoppingListId);

            modelBuilder.Entity<Entry>()
                .Property(x => x.Name)
                .HasMaxLength(35);

            modelBuilder.Entity<Entry>()
                .Property(x => x.CreatedOn)
                .HasDefaultValueSql("GetDate()");

            modelBuilder.Entity<User>()
                .Property(x => x.Name)
                .HasMaxLength(35);

            modelBuilder.Entity<User>()
                .Property(x => x.Email)
                .HasMaxLength(100);
        }
    }
}
