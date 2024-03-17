using MudBlazor;

namespace ShoppingList4.Blazor.Models
{
    public static class Common
    {
        public static DialogOptions GetDialogOptions()
        {
            return new DialogOptions()
            {
                FullWidth = true,
                MaxWidth = MaxWidth.ExtraSmall
            };
        }
    }
}
