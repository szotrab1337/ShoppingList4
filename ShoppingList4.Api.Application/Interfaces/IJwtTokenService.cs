using ShoppingList4.Api.Domain.Entities;

namespace ShoppingList4.Api.Application.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}