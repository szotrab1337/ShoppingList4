using ShoppingList4.Blazor.Dtos;
using ShoppingList4.Blazor.Interfaces;
using ShoppingList4.Domain.Entities;
using ShoppingList4.Domain.Interfaces;

namespace ShoppingList4.Blazor.Services
{
    public class EntryService(
        IEntryRepository entryRepository,
        IUserService userService,
        ILogger<EntryService> logger) : IEntryService
    {
        private readonly IEntryRepository _entryRepository = entryRepository;
        private readonly IUserService _userService = userService;
        private readonly ILogger<EntryService> _logger = logger;

        public async Task<IEnumerable<Entry>> GetShoppingListEntries(int shoppingListId)
        {
            var user = await _userService.GetCurrentUser();

            return await _entryRepository.GetByShoppingListId(user?.ApiToken, shoppingListId);
        }

        public async Task<Entry> Update(EditEntryDto dto)
        {
            var user = await _userService.GetCurrentUser();

            var result = await _entryRepository.Edit(
                user?.ApiToken,
                dto);

            _logger.LogInformation("User {@User} created new entry {@Entry}.",
                user, result.Item2);

            return result.Item2;
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _userService.GetCurrentUser();

            var result = await _entryRepository.Delete(
                user?.ApiToken,
                id);

            if (result)
            {
                _logger.LogInformation("{@User} deleted entry {EntryId}.",
                    user, id);
            }

            return result;
        }

        public async Task<bool> DeleteMultiple(IEnumerable<int> ids)
        {
            var user = await _userService.GetCurrentUser();

            var result = await _entryRepository.DeleteMultiple(
                user?.ApiToken,
                ids.ToArray());

            if (result)
            {
                _logger.LogInformation("{@User} deleted entries {@Ids}.",
                    user, ids);
            }

            return result;
        }

        public async Task<Entry> Add(AddEntryDto dto)
        {
            var user = await _userService.GetCurrentUser();

            var result = await _entryRepository.Add(
                user?.ApiToken,
                dto);

            _logger.LogInformation("{@User} created new entry {@Entry}.",
                user, result.Item2);

            return result.Item2;
        }
    }
}
