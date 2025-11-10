using Microsoft.Extensions.Logging;
using ShoppingList4.Application.Dtos;
using ShoppingList4.Application.Interfaces;
using ShoppingList4.Domain.Interfaces;
using Entry = ShoppingList4.Domain.Entities.Entry;

namespace ShoppingList4.Application.Services
{
    public class EntryService(
        IEntryRepository entryRepository,
        IUserService tokenService,
        ILogger<EntryService> logger) : IEntryService
    {
        private readonly IEntryRepository _entryRepository = entryRepository;
        private readonly ILogger<EntryService> _logger = logger;
        private readonly IUserService _userService = tokenService;

        public event EventHandler<Entry>? EntryAdded;
        public event EventHandler<int>? EntryDeleted;
        public event EventHandler<Entry>? EntryUpdated;

        public async Task Add(AddEntryDto dto)
        {
            var user = await _userService.GetCurrentUser();

            var result = await _entryRepository.Add(
                user?.ApiToken,
                dto);

            if (!result.Item1)
            {
                return;
            }

            EntryAdded?.Invoke(this, result.Item2);

            _logger.LogInformation("{@User} created new entry {@Entry}.",
                user, result.Item2);
        }

        public async Task Delete(int id)
        {
            var user = await _userService.GetCurrentUser();

            var result = await _entryRepository.Delete(
                user?.ApiToken,
                id);

            if (!result)
            {
                return;
            }

            EntryDeleted?.Invoke(this, id);

            _logger.LogInformation("{@User} deleted entry {EntryId}.",
                user, id);
        }

        public async Task DeleteMultiple(List<int> ids)
        {
            var user = await _userService.GetCurrentUser();

            var result = await _entryRepository.DeleteMultiple(
                user?.ApiToken,
                ids.ToArray());

            if (!result)
            {
                return;
            }

            foreach (var id in ids)
            {
                EntryDeleted?.Invoke(this, id);
            }

            _logger.LogInformation("{@User} deleted entries {@Ids}.",
                user, ids);
        }

        public async Task<IEnumerable<Entry>> GetShoppingListEntries(int shoppingListId)
        {
            var user = await _userService.GetCurrentUser();

            return await _entryRepository.GetByShoppingListId(user?.ApiToken, shoppingListId);
        }

        public async Task Update(EditEntryDto dto)
        {
            var user = await _userService.GetCurrentUser();

            var result = await _entryRepository.Edit(
                user?.ApiToken,
                dto);

            if (!result.Item1)
            {
                return;
            }

            EntryUpdated?.Invoke(this, result.Item2);

            _logger.LogInformation("User {@User} updated entry {@Entry}.",
                user, result.Item2);
        }
    }
}