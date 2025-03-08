using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShoppingList4.Maui.Dtos;
using ShoppingList4.Maui.Interfaces;
using ShoppingList4.Maui.ViewModel.Entities;
using System.ComponentModel.DataAnnotations;

namespace ShoppingList4.Maui.ViewModel
{
    public partial class EditEntryViewModel(
        IEntryService entryService,
        IMessageBoxService messageBoxService) : ObservableValidator, IQueryAttributable
    {
        private readonly IEntryService _entryService = entryService;
        private readonly IMessageBoxService _messageBoxService = messageBoxService;

        private int _id;

        [NotifyDataErrorInfo]
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        [MaxLength(35, ErrorMessage = "Wprowadzona nazwa jest zbyt długa")]
        [Required(ErrorMessage = "Pole wymagane")]
        private string _name = string.Empty;

        [RelayCommand(CanExecute = nameof(CanSave))]
        private async Task Save()
        {
            try
            {
                var dto = new EditEntryDto { Id = _id, Name = Name };
                var result = await _entryService.Update(dto);

                var navigationParam = new Dictionary<string, object> { { "EditedEntry", result } };
                await Shell.Current.GoToAsync("..", navigationParam);
            }
            catch (Exception)
            {
                await _messageBoxService.ShowAlert("Błąd", "Wystąpił błąd. Spróbuj ponownie.", "OK");
            }
        }

        private bool CanSave()
        {
            ValidateAllProperties();

            return !HasErrors;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var entry = (query["Entry"] as EntryViewModel)!;

            _id = entry.Id;
            Name = entry.Name;
        }
    }
}