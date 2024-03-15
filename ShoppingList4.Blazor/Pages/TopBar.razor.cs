
namespace ShoppingList4.Blazor.Pages
{
    public partial class TopBar
    {
        public bool TokenExists { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            TokenExists = await _tokenService.Exists();

            StateHasChanged();
        }

        public void Logout()
        {
            _tokenService.Remove();
            TokenExists = false;
        }
    }
}