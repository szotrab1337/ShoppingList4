using Blazored.LocalStorage;
using ShoppingList4.Blazor.Interfaces;
using ShoppingList4.Domain.Entities;

namespace ShoppingList4.Blazor.Services
{
    public class UserService(ILocalStorageService localStorageService) : IUserService
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;

        public async Task<bool> ExistsCurrentUser()
        {
            var user = await GetCurrentUser();

            return user is not null;
        }

        public async Task RemoveCurrentUser()
        {
            await _localStorageService.RemoveItemAsync("User");
        }

        public async Task SetCurrentUser(User user)
        {
            await _localStorageService.SetItemAsync("User", user);
        }

        public async Task<User?> GetCurrentUser()
        {
            return await _localStorageService.GetItemAsync<User>("User");
        }
    }
}