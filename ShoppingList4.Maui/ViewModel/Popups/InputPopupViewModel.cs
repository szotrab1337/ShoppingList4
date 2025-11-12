using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ShoppingList4.Maui.ViewModel.Popups
{
    public partial class InputPopupViewModel(IPopupService popupService) : ObservableValidator, IQueryAttributable
    {
        private readonly IPopupService _popupService = popupService;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        [MinLength(1)]
        [Required]
        [NotifyDataErrorInfo]
        private string _text = string.Empty;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Text = (string)query[nameof(Text)];
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Text);
        }

        [RelayCommand(CanExecute = nameof(CanSave))]
        private async Task OnSave()
        {
            await _popupService.ClosePopupAsync(Shell.Current, Text);
        }
    }
}