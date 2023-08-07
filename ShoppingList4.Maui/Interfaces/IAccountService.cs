#nullable enable
using ShoppingList4.Maui.Entity;

namespace ShoppingList4.Maui.Interfaces;

public interface IAccountService
{
    Task LoginAsync(User user);
}