using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ShoppingList4.Blazor.Pages.Dialogs
{
    public partial class SimpleDialog
    {
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
        [Parameter] public string Text { get; set; } = null!;
        [Parameter] public string Title { get; set; } = null!;

        public void Cancel() => MudDialog.Cancel();

        public void Save()
        {
            MudDialog.Close(DialogResult.Ok(Text));
        }
    }
}