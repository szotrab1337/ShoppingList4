using Microsoft.Extensions.Logging;
using ShoppingList4.Domain.Entities;
using ShoppingList4.Domain.Interfaces;
using ShoppingList4.Maui.Dtos;
using ShoppingList4.Maui.Interfaces;

namespace ShoppingList4.Maui.Services
{
    public class ShoppingListService(
        IShoppingListRepository shoppingListRepository,
        IUserService userService,
        ILogger<ShoppingListService> logger) : IShoppingListService
    {
        private readonly IShoppingListRepository _shoppingListRepository = shoppingListRepository;
        private readonly IUserService _userService = userService;
        private readonly ILogger<ShoppingListService> _logger = logger;

        public async Task<IEnumerable<ShoppingList>> GetAll()
        {
            var user = await _userService.GetCurrentUser();

            return await _shoppingListRepository.GetAll(user?.ApiToken);
        }

        public async Task<ShoppingList> Add(AddShoppingListDto dto)
        {
            var user = await _userService.GetCurrentUser();

            var result = await _shoppingListRepository.Add(
                user?.ApiToken,
                dto);

            _logger.LogInformation("{@User} created new shopping list {@ShoppingList}.",
                user, result.Item2);

            return result.Item2;
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _userService.GetCurrentUser();

            var result = await _shoppingListRepository.Delete(
                user?.ApiToken,
                id);

            if (result)
            {
                _logger.LogInformation("{@User} deleted shopping list {ShoppingList}.",
                    user, id);
            }

            return result;
        }

        public async Task<ShoppingList> Update(EditShoppingListDto dto)
        {
            var user = await _userService.GetCurrentUser();

            var result = await _shoppingListRepository.Edit(
                user?.ApiToken,
                dto);

            _logger.LogInformation("User {@User} created new entry {@Entry}.",
                user, result.Item2);

            return result.Item2;
        }
    }
}
