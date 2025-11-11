using ShoppingList4.Application.Interfaces;
using ShoppingList4.Domain.Interfaces;

namespace ShoppingList4.Application.Services
{
    public class AccountService(
        IUsersRepository usersRepository,
        IUserService userService) : IAccountService
    {
        private readonly IUserService _userService = userService;
        private readonly IUsersRepository _usersRepository = usersRepository;

        public async Task Login(string email, string password)
        {
            var user = await _usersRepository.Login(email, password);

            if (user is null)
            {
                _userService.RemoveCurrentUser();
                return;
            }

            await _userService.SetCurrentUser(user);
        }
    }
}