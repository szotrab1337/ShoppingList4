using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ShoppingList4.Blazor.Components
{
    public partial class KeyboardShortcuts : IAsyncDisposable
    {
        [Parameter]
        public string ShortcutKey { get; set; } = default!;

        [Parameter]
        public EventCallback OnShortcutPressed { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("keyboardShortcuts.registerShortcut", ShortcutKey, DotNetObjectReference.Create(this));
            }
        }

        [JSInvokable]
        public async Task InvokeShortcut()
        {
            await OnShortcutPressed.InvokeAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await JSRuntime.InvokeVoidAsync("keyboardShortcuts.unregisterShortcut", ShortcutKey, DotNetObjectReference.Create(this));
        }
    }
}