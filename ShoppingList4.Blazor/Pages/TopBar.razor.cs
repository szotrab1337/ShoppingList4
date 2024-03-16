using Microsoft.AspNetCore.Components;
using ShoppingList4.Blazor.Interfaces;

namespace ShoppingList4.Blazor.Pages
{
    public partial class TopBar
    {
        [Inject] public ITokenService TokenService { get; set; } = default!;

        public bool TokenExists { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            TokenExists = await TokenService.Exists();

            StateHasChanged();
        }

        public void Logout()
        {
            TokenService.Remove();
            TokenExists = false;
        }
    }
}