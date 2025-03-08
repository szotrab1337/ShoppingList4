using ShoppingList4.Domain.Entities;
using ShoppingList4.Maui.Interfaces;
using System.Text.Json;

namespace ShoppingList4.Maui.Services
{
    public class UserService : IUserService
    {
        public async Task<bool> ExistsCurrentUser()
        {
            var user = await SecureStorage.GetAsync("User");

            return !string.IsNullOrEmpty(user);
        }

        public void RemoveCurrentUser()
        {
            SecureStorage.Remove("User");
        }

        public async Task SetCurrentUser(User user)
        {
            await SecureStorage.SetAsync("User", JsonSerializer.Serialize(user));
        }

        public async Task<User?> GetCurrentUser()
        {
            var user = await SecureStorage.GetAsync("User");

            return string.IsNullOrEmpty(user) ? null : JsonSerializer.Deserialize<User>(user);
        }
    }
}