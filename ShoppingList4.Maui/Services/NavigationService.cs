using ShoppingList4.Maui.Interfaces;

namespace ShoppingList4.Maui.Services
{
    public class NavigationService : INavigationService
    {
        public async Task NavigateTo(string route)
        {
            await Shell.Current.GoToAsync(route);
        }

        public async Task NavigateTo(ShellNavigationState state, IDictionary<string, object> parameter)
        {
            await Shell.Current.GoToAsync(state, parameter);
        }
    }
}