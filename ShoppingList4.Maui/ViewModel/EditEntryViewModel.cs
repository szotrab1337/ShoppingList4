using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShoppingList4.Maui.Interfaces;
using System.ComponentModel.DataAnnotations;
using Entry = ShoppingList4.Maui.Entity.Entry;

namespace ShoppingList4.Maui.ViewModel
{
    public partial class EditEntryViewModel : ObservableValidator, IQueryAttributable
    {
        private readonly IEntryService _entryService;
        private readonly IMessageBoxService _messageBoxService;

        public EditEntryViewModel(IEntryService entryService, IMessageBoxService messageBoxService)
        {
            _entryService = entryService;
            _messageBoxService = messageBoxService;

            SaveAsyncCommand = new AsyncRelayCommand(SaveAsync, CanSave);
        }

        public IAsyncRelayCommand SaveAsyncCommand { get; }

        [ObservableProperty]
        private Entry _entry;

        [MaxLength(35, ErrorMessage = "Wprowadzona nazwa jest zbyt długa")]
        [Required(ErrorMessage = "Pole wymagane")]
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                ValidateProperty(_name);
                SaveAsyncCommand.NotifyCanExecuteChanged();
            }
        }
        private string _name;

        partial void OnEntryChanged(Entry value)
        {
            Name = value.Name;
        }

        private async Task SaveAsync()
        {
            try
            {
                Entry.Name = Name;
                var result = await _entryService.UpdateAsync(Entry);

                if (result)
                {
                    await Shell.Current.GoToAsync("..");
                }
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
            Entry = (query["Entry"] as Entry)!;
        }
    }
}
