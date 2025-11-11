using Microsoft.Extensions.Logging;
using ShoppingList4.Application.Dtos;
using ShoppingList4.Application.Interfaces;
using ShoppingList4.Domain.Entities;
using ShoppingList4.Domain.Interfaces;

namespace ShoppingList4.Application.Services
{
    public class ShoppingListService(
        IShoppingListRepository shoppingListRepository,
        IUserService userService,
        ILogger<ShoppingListService> logger) : IShoppingListService
    {
        private readonly ILogger<ShoppingListService> _logger = logger;
        private readonly IShoppingListRepository _shoppingListRepository = shoppingListRepository;
        private readonly IUserService _userService = userService;

        public event EventHandler<ShoppingList>? ShoppingListAdded;
        public event EventHandler<int>? ShoppingListDeleted;
        public event EventHandler<ShoppingList>? ShoppingListUpdated;

        public async Task Add(AddShoppingListDto dto)
        {
            var user = await _userService.GetCurrentUser();

            var result = await _shoppingListRepository.Add(
                user?.ApiToken,
                dto);

            if (!result.Item1)
            {
                return;
            }

            _logger.LogInformation("{@User} created new shopping list {@ShoppingList}.",
                user, result.Item2);

            ShoppingListAdded?.Invoke(this, result.Item2);
        }

        public async Task Delete(int id)
        {
            var user = await _userService.GetCurrentUser();

            var result = await _shoppingListRepository.Delete(
                user?.ApiToken,
                id);

            if (!result)
            {
                return;
            }

            _logger.LogInformation("{@User} deleted shopping list {ShoppingList}.",
                user, id);

            ShoppingListDeleted?.Invoke(this, id);
        }

        public async Task<IEnumerable<ShoppingList>> GetAll()
        {
            var user = await _userService.GetCurrentUser();

            return await _shoppingListRepository.GetAll(user?.ApiToken);
        }

        public async Task Update(EditShoppingListDto dto)
        {
            var user = await _userService.GetCurrentUser();

            var result = await _shoppingListRepository.Edit(
                user?.ApiToken,
                dto);

            if (!result.Item1)
            {
                return;
            }

            _logger.LogInformation("User {@User} created new entry {@Entry}.",
                user, result.Item2);

            ShoppingListUpdated?.Invoke(this, result.Item2);
        }
    }
}