namespace ShoppingList4.Maui.Interfaces
{
    public interface INavigationService
    {
        Task NavigateTo(string route);
        Task NavigateTo(ShellNavigationState state, IDictionary<string ,object> parameter);
    }
}