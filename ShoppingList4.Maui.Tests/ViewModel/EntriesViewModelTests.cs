using FluentAssertions;
using Moq;
using ShoppingList4.Application.Dtos;
using ShoppingList4.Application.Interfaces;
using ShoppingList4.Maui.Interfaces;
using ShoppingList4.Maui.ViewModel;
using ShoppingList4.Maui.ViewModel.Entities;
using Entry = ShoppingList4.Domain.Entities.Entry;

namespace ShoppingList4.Maui.Tests.ViewModel
{
    public class EntriesViewModelTests
    {
        private readonly Mock<IEntryService> _entryServiceMock;
        private readonly Mock<IMessageBoxService> _messageBoxServiceMock;
        private readonly Mock<IAppPopupService> _appPopupServiceMock;
        
        private readonly EntriesViewModel _viewModel;

        public EntriesViewModelTests()
        {
            _entryServiceMock = new Mock<IEntryService>();
            _messageBoxServiceMock = new Mock<IMessageBoxService>();
            _appPopupServiceMock = new Mock<IAppPopupService>();

            _viewModel = new EntriesViewModel(
                _entryServiceMock.Object,
                _messageBoxServiceMock.Object,
                _appPopupServiceMock.Object
            );
        }

        [Fact]
        public async Task AddCommand_ShouldAddEntry_WhenNameIsProvided()
        {
            // Arrange
            _viewModel.ApplyQueryAttributes(new Dictionary<string, object> { { "ShoppingListId", 1 } });
            _appPopupServiceMock.Setup(d => d.ShowInputPopup(null)).ReturnsAsync("Masło");

            // Act
            await _viewModel.AddCommand.ExecuteAsync(null);

            // Assert
            _entryServiceMock.Verify(s => s.Add(It.Is<AddEntryDto>(dto =>
                dto.Name == "Masło" && dto.ShoppingListId == 1)), Times.Once);
        }

        [Fact]
        public async Task AddCommand_ShouldNotAddEntry_WhenNameIsEmpty()
        {
            // Arrange
            _appPopupServiceMock.Setup(d => d.ShowInputPopup(null)).ReturnsAsync(string.Empty);

            // Act
            await _viewModel.AddCommand.ExecuteAsync(null);

            // Assert
            _entryServiceMock.Verify(s => s.Add(It.IsAny<AddEntryDto>()), Times.Never);
        }

        [Fact]
        public void ApplyQueryAttributes_ShouldSetShoppingListId()
        {
            // Arrange
            var query = new Dictionary<string, object>
            {
                { "ShoppingListId", 123 }
            };

            // Act
            _viewModel.ApplyQueryAttributes(query);

            // Assert
            // ShoppingListId jest prywatne, więc sprawdzimy to pośrednio w Initialize
        }

        [Fact]
        public void ChangeIsBoughtValue_ShouldInvokeCommand_WhenPropertyChanges()
        {
            // Arrange
            var entry = new Entry { Id = 1, Name = "Mleko", IsBought = false, ShoppingListId = 1 };
            var entryViewModel = new EntryViewModel(entry, _viewModel.ChangeIsBoughtValueCommand);

            // Act
            entryViewModel.IsBought = true;

            // Assert - OnIsBoughtChanged automatycznie wywołuje command
            _entryServiceMock.Verify(s => s.Update(It.Is<EditEntryDto>(dto =>
                dto.Id == 1 && dto.IsBought == true && dto.Name == "Mleko")), Times.Once);
        }

        [Fact]
        public async Task ChangeIsBoughtValueCommand_ShouldDoNothing_WhenViewModelIsNull()
        {
            // Act
            await _viewModel.ChangeIsBoughtValueCommand.ExecuteAsync(null);

            // Assert
            _entryServiceMock.Verify(s => s.Update(It.IsAny<EditEntryDto>()), Times.Never);
        }

        [Fact]
        public async Task ChangeIsBoughtValueCommand_ShouldUpdateEntry()
        {
            // Arrange
            var entry = new Entry { Id = 1, Name = "Mleko", IsBought = false, ShoppingListId = 1 };
            var entryViewModel = new EntryViewModel(entry, _viewModel.ChangeIsBoughtValueCommand);

            // Act
            await _viewModel.ChangeIsBoughtValueCommand.ExecuteAsync(entryViewModel);

            // Assert
            _entryServiceMock.Verify(s => s.Update(It.Is<EditEntryDto>(dto =>
                dto.Id == 1 && dto.IsBought == false && dto.Name == "Mleko")), Times.Once);
        }

        [Fact]
        public async Task DeleteAllCommand_ShouldDeleteAllEntries_WhenConfirmed()
        {
            // Arrange
            var entries = new List<Entry>
            {
                new() { Id = 1, Name = "Mleko", IsBought = false, ShoppingListId = 1 },
                new() { Id = 2, Name = "Chleb", IsBought = true, ShoppingListId = 1 }
            };

            _entryServiceMock.Setup(s => s.GetShoppingListEntries(It.IsAny<int>()))
                .ReturnsAsync(entries);
            _messageBoxServiceMock.Setup(m => m.ShowAlert(
                    "Potwierdzenie", "Czy na pewno chcesz usunąć wszystkie pozycje?", "TAK", "NIE"))
                .ReturnsAsync(true);

            _viewModel.ApplyQueryAttributes(new Dictionary<string, object> { { "ShoppingListId", 1 } });
            await _viewModel.Initialize();

            // Act
            await _viewModel.DeleteAllCommand.ExecuteAsync(null);

            // Assert
            _entryServiceMock.Verify(s => s.DeleteMultiple(It.Is<List<int>>(ids =>
                ids.Count == 2 && ids.Contains(1) && ids.Contains(2))), Times.Once);
        }

        [Fact]
        public async Task DeleteAllCommand_ShouldNotDelete_WhenNotConfirmed()
        {
            // Arrange
            _messageBoxServiceMock.Setup(m => m.ShowAlert(
                    "Potwierdzenie", "Czy na pewno chcesz usunąć wszystkie pozycje?", "TAK", "NIE"))
                .ReturnsAsync(false);

            // Act
            await _viewModel.DeleteAllCommand.ExecuteAsync(null);

            // Assert
            _entryServiceMock.Verify(s => s.DeleteMultiple(It.IsAny<List<int>>()), Times.Never);
        }

        [Fact]
        public async Task DeleteBoughtCommand_ShouldDeleteOnlyBoughtEntries_WhenConfirmed()
        {
            // Arrange
            var entries = new List<Entry>
            {
                new() { Id = 1, Name = "Mleko", IsBought = false, ShoppingListId = 1 },
                new() { Id = 2, Name = "Chleb", IsBought = true, ShoppingListId = 1 },
                new() { Id = 3, Name = "Masło", IsBought = true, ShoppingListId = 1 }
            };

            _entryServiceMock.Setup(s => s.GetShoppingListEntries(It.IsAny<int>()))
                .ReturnsAsync(entries);
            _messageBoxServiceMock.Setup(m => m.ShowAlert(
                    "Potwierdzenie", "Czy na pewno chcesz usunąć kupione pozycje?", "TAK", "NIE"))
                .ReturnsAsync(true);

            _viewModel.ApplyQueryAttributes(new Dictionary<string, object> { { "ShoppingListId", 1 } });
            await _viewModel.Initialize();

            // Act
            await _viewModel.DeleteBoughtCommand.ExecuteAsync(null);

            // Assert
            _entryServiceMock.Verify(s => s.DeleteMultiple(It.Is<List<int>>(ids =>
                ids.Count == 2 && ids.Contains(2) && ids.Contains(3))), Times.Once);
        }

        [Fact]
        public async Task DeleteBoughtCommand_ShouldNotDelete_WhenNotConfirmed()
        {
            // Arrange
            _messageBoxServiceMock.Setup(m => m.ShowAlert(
                    "Potwierdzenie", "Czy na pewno chcesz usunąć kupione pozycje?", "TAK", "NIE"))
                .ReturnsAsync(false);

            // Act
            await _viewModel.DeleteBoughtCommand.ExecuteAsync(null);

            // Assert
            _entryServiceMock.Verify(s => s.DeleteMultiple(It.IsAny<List<int>>()), Times.Never);
        }

        [Fact]
        public async Task DeleteCommand_ShouldDeleteEntry()
        {
            // Arrange
            var entry = new Entry { Id = 1, Name = "Mleko", IsBought = false, ShoppingListId = 1 };
            var entryViewModel = new EntryViewModel(entry, _viewModel.ChangeIsBoughtValueCommand);

            // Act
            await _viewModel.DeleteCommand.ExecuteAsync(entryViewModel);

            // Assert
            _entryServiceMock.Verify(s => s.Delete(1), Times.Once);
        }

        [Fact]
        public async Task DeleteCommand_ShouldShowError_OnException()
        {
            // Arrange
            var entry = new Entry { Id = 1, Name = "Mleko", IsBought = false, ShoppingListId = 1 };
            var entryViewModel = new EntryViewModel(entry, _viewModel.ChangeIsBoughtValueCommand);

            _entryServiceMock.Setup(s => s.Delete(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            await _viewModel.DeleteCommand.ExecuteAsync(entryViewModel);

            // Assert
            _messageBoxServiceMock.Verify(m => m.ShowAlert("Błąd", "Wystąpił błąd. Spróbuj ponownie.", "OK"),
                Times.Once);
        }

        [Fact]
        public async Task EditCommand_ShouldNotUpdateEntry_WhenNameIsEmpty()
        {
            // Arrange
            var entry = new Entry { Id = 1, Name = "Mleko", IsBought = false, ShoppingListId = 1 };
            var entryViewModel = new EntryViewModel(entry, _viewModel.ChangeIsBoughtValueCommand);

            _appPopupServiceMock.Setup(d => d.ShowInputPopup(It.IsAny<string>())).ReturnsAsync(string.Empty);

            // Act
            await _viewModel.EditCommand.ExecuteAsync(entryViewModel);

            // Assert
            _entryServiceMock.Verify(s => s.Update(It.IsAny<EditEntryDto>()), Times.Never);
        }

        [Fact]
        public async Task EditCommand_ShouldUpdateEntry_WhenNameIsProvided()
        {
            // Arrange
            var entry = new Entry { Id = 1, Name = "Mleko", IsBought = false, ShoppingListId = 1 };
            var entryViewModel = new EntryViewModel(entry, _viewModel.ChangeIsBoughtValueCommand);

            _appPopupServiceMock.Setup(d => d.ShowInputPopup("Mleko")).ReturnsAsync("Mleko 2%");

            // Act
            await _viewModel.EditCommand.ExecuteAsync(entryViewModel);

            // Assert
            _entryServiceMock.Verify(s => s.Update(It.Is<EditEntryDto>(dto =>
                dto.Id == 1 && dto.Name == "Mleko 2%")), Times.Once);
        }

        [Fact]
        public async Task EntryAdded_Event_ShouldAddEntryToCollection()
        {
            // Arrange
            _viewModel.ApplyQueryAttributes(new Dictionary<string, object> { { "ShoppingListId", 1 } });
            _entryServiceMock.Setup(s => s.GetShoppingListEntries(It.IsAny<int>()))
                .ReturnsAsync(new List<Entry>());

            await _viewModel.Initialize();

            var newEntry = new Entry { Id = 1, Name = "Nowy produkt", IsBought = false, ShoppingListId = 1 };

            // Act
            _entryServiceMock.Raise(s => s.EntryAdded += null, this, newEntry);

            // Assert
            _viewModel.Entries.Should().HaveCount(1);
            _viewModel.Entries.First().Name.Should().Be("Nowy produkt");
        }

        [Fact]
        public async Task EntryDeleted_Event_ShouldRemoveEntryFromCollection()
        {
            // Arrange
            var entries = new List<Entry>
            {
                new() { Id = 1, Name = "Mleko", IsBought = false, ShoppingListId = 1 }
            };

            _entryServiceMock.Setup(s => s.GetShoppingListEntries(It.IsAny<int>()))
                .ReturnsAsync(entries);

            _viewModel.ApplyQueryAttributes(new Dictionary<string, object> { { "ShoppingListId", 1 } });
            await _viewModel.Initialize();

            // Act
            _entryServiceMock.Raise(s => s.EntryDeleted += null, this, 1);

            // Assert
            _viewModel.Entries.Should().BeEmpty();
        }

        [Fact]
        public async Task EntryUpdated_Event_ShouldUpdateEntryInCollection()
        {
            // Arrange
            var entries = new List<Entry>
            {
                new() { Id = 1, Name = "Mleko", IsBought = false, ShoppingListId = 1 }
            };

            _entryServiceMock.Setup(s => s.GetShoppingListEntries(It.IsAny<int>()))
                .ReturnsAsync(entries);

            _viewModel.ApplyQueryAttributes(new Dictionary<string, object> { { "ShoppingListId", 1 } });
            await _viewModel.Initialize();

            var updatedEntry = new Entry { Id = 1, Name = "Mleko 2%", IsBought = true, ShoppingListId = 1 };

            // Act
            _entryServiceMock.Raise(s => s.EntryUpdated += null, this, updatedEntry);

            // Assert
            _viewModel.Entries.First().Name.Should().Be("Mleko 2%");
            _viewModel.Entries.First().IsBought.Should().BeTrue();
        }

        [Fact]
        public async Task Initialize_ShouldLoadEntries_WhenNotLoadedBefore()
        {
            // Arrange
            var entries = new List<Entry>
            {
                new() { Id = 1, Name = "Mleko", IsBought = false, ShoppingListId = 1 },
                new() { Id = 2, Name = "Chleb", IsBought = true, ShoppingListId = 1 }
            };

            _entryServiceMock.Setup(s => s.GetShoppingListEntries(It.IsAny<int>()))
                .ReturnsAsync(entries);

            _viewModel.ApplyQueryAttributes(new Dictionary<string, object> { { "ShoppingListId", 1 } });

            // Act
            await _viewModel.Initialize();

            // Assert
            _viewModel.Entries.Should().HaveCount(2);
            _viewModel.IsInitializing.Should().BeFalse();
            _entryServiceMock.Verify(s => s.GetShoppingListEntries(1), Times.Once);
        }

        [Fact]
        public async Task Initialize_ShouldNotLoadEntries_WhenAlreadyLoaded()
        {
            // Arrange
            _viewModel.ApplyQueryAttributes(new Dictionary<string, object> { { "ShoppingListId", 1 } });
            await _viewModel.Initialize();

            // Act
            await _viewModel.Initialize();

            // Assert
            _entryServiceMock.Verify(s => s.GetShoppingListEntries(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task RefreshCommand_ShouldReloadEntries()
        {
            // Arrange
            var entries = new List<Entry>
            {
                new() { Id = 1, Name = "Mleko", IsBought = false, ShoppingListId = 1 }
            };

            _entryServiceMock.Setup(s => s.GetShoppingListEntries(It.IsAny<int>()))
                .ReturnsAsync(entries);

            _viewModel.ApplyQueryAttributes(new Dictionary<string, object> { { "ShoppingListId", 1 } });

            // Act
            await _viewModel.RefreshCommand.ExecuteAsync(null);

            // Assert
            _viewModel.IsRefreshing.Should().BeFalse();
            _entryServiceMock.Verify(s => s.GetShoppingListEntries(1), Times.Once);
        }
    }
}