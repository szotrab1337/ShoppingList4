using MudBlazor;
using ShoppingList4.Blazor.Entity;

namespace ShoppingList4.Blazor.Pages
{
    public partial class Login
    {
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

                await _accountService.LoginAsync(user);

                _navigationManager.NavigateTo("/");
            }
        }
    }
}