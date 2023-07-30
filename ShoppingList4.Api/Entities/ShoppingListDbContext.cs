using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8618

namespace ShoppingList4.Api.Entities
{
    public class ShoppingListDbContext : DbContext
    {
        public ShoppingListDbContext(DbContextOptions<ShoppingListDbContext> options) : base(options)
        {
            
        }

        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<User> Users { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingList>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(35);

            modelBuilder.Entity<ShoppingList>()
                .Property(x => x.CreatedOn)
                .IsRequired();

            modelBuilder.Entity<Entry>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(35);

            modelBuilder.Entity<Entry>()
                .Property(x => x.CreatedOn)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(35);

            modelBuilder.Entity<User>()
                .Property(x => x.Email)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(x => x.PasswordHash)
                .IsRequired();
        }
    }
}
