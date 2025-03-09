using Microsoft.AspNetCore.Components;
using ShoppingList4.Blazor.Interfaces;

namespace ShoppingList4.Blazor.Pages
{
    public partial class TopBar
    {
        [Inject] public IUserService UserService { get; set; } = null!;

        private bool UserExists { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            UserExists = await UserService.ExistsCurrentUser();

            StateHasChanged();
        }

        public async Task Logout()
        {
            await UserService.RemoveCurrentUser();
            UserExists = false;
        }
    }
}