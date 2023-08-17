using Microsoft.EntityFrameworkCore;
using ShoppingList4.Api.Entities;
using ShoppingList4.Api.Interfaces;

namespace ShoppingList4.Api.Services
{
    public class DbManager : IDbManager
    {
        private readonly ShoppingListDbContext _dbContext;

        public DbManager(ShoppingListDbContext dbContext)
        {
            _dbContext = dbContext;

            Migrate();
        }

        public void Migrate()
        {
            if (!_dbContext.Database.CanConnect())
            {
                return;
            }

            var pendingMigrations = _dbContext.Database.GetPendingMigrations();

            if (pendingMigrations.Any())
            {
                _dbContext.Database.Migrate();
            }
        }
    }
}
