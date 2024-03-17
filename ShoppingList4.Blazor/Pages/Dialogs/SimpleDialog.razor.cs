using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ShoppingList4.Blazor.Pages.Dialogs
{
    public partial class SimpleDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;
        [Parameter] public string Text { get; set; } = default!;
        [Parameter] public string Title { get; set; } = default!;

        public void Cancel() => MudDialog.Cancel();

        public void Save()
        {
            MudDialog.Close(DialogResult.Ok(Text));
        }
    }
}