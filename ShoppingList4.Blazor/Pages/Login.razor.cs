using Microsoft.AspNetCore.Components;
using MudBlazor;
using ShoppingList4.Blazor.Interfaces;

namespace ShoppingList4.Blazor.Pages
{
    public partial class Login
    {
        [Inject] public IAccountService AccountService { get; set; } = null!;
        [Inject] public IUserService UserService { get; set; } = null!;
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        private bool Success { get; set; }
        public MudForm Form { get; set; } = null!;

        private async Task Submit()
        {
            try
            {
                await AccountService.Login(Email, Password);

                if (!await UserService.ExistsCurrentUser())
                {
                    return;
                }

                NavigationManager.NavigateTo("/");
            }
            catch
            {
                // ignored
            }
        }
    }
}