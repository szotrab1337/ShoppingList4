using CommunityToolkit.Maui;
using Microsoft.Maui.Controls.Shapes;
using ShoppingList4.Maui.Interfaces;
using ShoppingList4.Maui.ViewModel.Popups;

namespace ShoppingList4.Maui.Services
{
    public class AppPopupService(IPopupService popupService) : IAppPopupService
    {
        private readonly IPopupService _popupService = popupService;
        public event EventHandler<bool>? PopupVisibilityChanged;

        public async Task<string> ShowInputPopup(string value)
        {
            var queryAttributes = new Dictionary<string, object>
            {
                [nameof(InputPopupViewModel.Text)] = value
            };

            var popupOptions = new PopupOptions
            {
                Shape = new RoundRectangle
                {
                    StrokeThickness = 1,
                    Stroke = Color.FromArgb("#FF512BD4"),
                    CornerRadius = 10,
                    MinimumWidthRequest = 900,
                    Margin = 0
                }
            };

            PopupVisibilityChanged?.Invoke(this, true);

            var result = await _popupService.ShowPopupAsync<InputPopupViewModel, string>(
                Shell.Current,
                popupOptions,
                queryAttributes);

            PopupVisibilityChanged?.Invoke(this, false);

            if (result.WasDismissedByTappingOutsideOfPopup || string.IsNullOrEmpty(result.Result))
            {
                return string.Empty;
            }

            return result.Result;
        }
    }
}