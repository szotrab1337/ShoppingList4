using System.Text.Json;
using ShoppingList4.Application.Interfaces;
using ShoppingList4.Domain.Entities;

namespace ShoppingList4.Maui.Services
{
    public class UserService : IUserService
    {
        public async Task<bool> ExistsCurrentUser()
        {
            var user = await SecureStorage.GetAsync("User");

            return !string.IsNullOrEmpty(user);
        }

        public async Task<User?> GetCurrentUser()
        {
            var user = await SecureStorage.GetAsync("User");

            return string.IsNullOrEmpty(user) ? null : JsonSerializer.Deserialize<User>(user);
        }

        public void RemoveCurrentUser()
        {
            SecureStorage.Remove("User");
        }

        public async Task SetCurrentUser(User user)
        {
            await SecureStorage.SetAsync("User", JsonSerializer.Serialize(user));
        }
    }
}