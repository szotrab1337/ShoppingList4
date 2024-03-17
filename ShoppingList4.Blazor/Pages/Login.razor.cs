using Microsoft.AspNetCore.Components;
using MudBlazor;
using ShoppingList4.Blazor.Entities;
using ShoppingList4.Blazor.Interfaces;

namespace ShoppingList4.Blazor.Pages
{
    public partial class Login
    {
        [Inject] public IAccountService AccountService { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool Success { get; set; }
        public MudForm Form { get; set; } = default!;

        private async Task Submit()
        {
            await Form.Validate();

            if (Form.IsValid)
            {
                var user = new User()
                {
                    Email = Email,
                    Password = Password
                };

                await AccountService.LoginAsync(user);

                NavigationManager.NavigateTo("/");
            }
        }
    }
}