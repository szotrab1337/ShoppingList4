using ShoppingList4.Api.Models;

namespace ShoppingList4.Api.Interfaces;

public interface IAccountService
{
    void RegisterUser(RegisterUserDto dto);
    string GenerateJwt(LoginDto dto);
}