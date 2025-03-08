using ShoppingList4.Domain.Interfaces;
using ShoppingList4.Maui.Interfaces;

namespace ShoppingList4.Maui.Services
{
    public class AccountService(
        IUsersRepository usersRepository,
        IUserService userService) : IAccountService
    {
        private readonly IUsersRepository _usersRepository = usersRepository;
        private readonly IUserService _userService = userService;

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
