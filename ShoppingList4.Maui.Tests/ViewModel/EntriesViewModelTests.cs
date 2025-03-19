using CommunityToolkit.Maui.Views;
using FluentAssertions;
using Moq;
using ShoppingList4.Maui.Dtos;
using ShoppingList4.Maui.Interfaces;
using ShoppingList4.Maui.ViewModel;
using ShoppingList4.Maui.ViewModel.Entities;
using System.Collections.ObjectModel;

namespace ShoppingList4.Maui.Tests.ViewModel
{
    public class EntriesViewModelTests
    {
        private readonly Mock<IEntryService> _entryServiceMock;
        private readonly Mock<IMessageBoxService> _messageBoxServiceMock;
        private readonly Mock<IDialogService> _dialogServiceMock;
        private readonly EntriesViewModel _viewModel;

        public EntriesViewModelTests()
        {
            _entryServiceMock = new Mock<IEntryService>();
            _messageBoxServiceMock = new Mock<IMessageBoxService>();
            _dialogServiceMock = new Mock<IDialogService>();

            _viewModel = new EntriesViewModel(
                _entryServiceMock.Object,
                _messageBoxServiceMock.Object,
                _dialogServiceMock.Object);
        }

        [Fact]
        public async Task Initialize_ShouldLoadEntriesOnce()
        {
            // Arrange
            _entryServiceMock
                .Setup(s => s.GetShoppingListEntries(It.IsAny<int>()))
                .ReturnsAsync(new List<ShoppingList4.Domain.Entities.Entry>());

            // Act
            await _viewModel.Initialize();
            await _viewModel.Initialize(); // Druga inicjalizacja nie powinna pobierać danych ponownie

            // Assert
            _entryServiceMock.Verify(s => s.GetShoppingListEntries(It.IsAny<int>()), Times.Once);
            _viewModel.IsInitializing.Should().BeFalse();
        }

        [Fact]
        public async Task RefreshCommand_ShouldReloadEntries()
        {
            // Arrange
            var entries = new List<ShoppingList4.Domain.Entities.Entry>
            {
                new() { Id = 1, Name = "Milk", IsBought = false }, new() { Id = 2, Name = "Bread", IsBought = true }
            };

            _entryServiceMock
                .Setup(s => s.GetShoppingListEntries(It.IsAny<int>()))
                .ReturnsAsync(entries);

            // Act
            await _viewModel.RefreshCommand.ExecuteAsync(null);

            // Assert
            _viewModel.Entries.Should().HaveCount(2);
            _viewModel.IsRefreshing.Should().BeFalse();
        }

        [Fact]
        public async Task AddCommand_ShouldInsertNewEntry_WhenDialogReturnsName()
        {
            // Arrange
            _dialogServiceMock
                .Setup(d => d.ShowPromptAsync(It.IsAny<Popup>()))
                .ReturnsAsync("Apples");

            var newEntry = new ShoppingList4.Domain.Entities.Entry { Id = 3, Name = "Apples", IsBought = false };

            _entryServiceMock
                .Setup(s => s.Add(It.IsAny<AddEntryDto>()))
                .ReturnsAsync(newEntry);

            // Act
            await _viewModel.AddCommand.ExecuteAsync(null);

            // Assert
            _viewModel.Entries.Should().ContainSingle(e => e.Name == "Apples");
        }

        [Fact]
        public async Task DeleteCommand_ShouldRemoveEntry_WhenServiceReturnsTrue()
        {
            // Arrange
            var entry = new EntryViewModel(new ShoppingList4.Domain.Entities.Entry { Id = 1, Name = "Eggs" });
            _viewModel.Entries = [entry];

            _entryServiceMock
                .Setup(s => s.Delete(entry.Id))
                .ReturnsAsync(true);

            // Act
            await _viewModel.DeleteCommand.ExecuteAsync(entry);

            // Assert
            _viewModel.Entries.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteCommand_ShouldNotRemoveEntry_WhenServiceReturnsFalse()
        {
            // Arrange
            var entry = new EntryViewModel(new ShoppingList4.Domain.Entities.Entry { Id = 1, Name = "Eggs" });
            _viewModel.Entries = [entry];

            _entryServiceMock
                .Setup(s => s.Delete(entry.Id))
                .ReturnsAsync(false);

            // Act
            await _viewModel.DeleteCommand.ExecuteAsync(entry);

            // Assert
            _viewModel.Entries.Should().ContainSingle(e => e.Name == "Eggs");
        }

        [Fact]
        public async Task EditCommand_ShouldUpdateEntry_WhenDialogReturnsNewName()
        {
            // Arrange
            var entry = new EntryViewModel(new ShoppingList4.Domain.Entities.Entry { Id = 1, Name = "OldName" });
            _viewModel.Entries = [entry];

            _dialogServiceMock
                .Setup(d => d.ShowPromptAsync(It.IsAny<Popup>()))
                .ReturnsAsync("NewName");

            var updatedEntry = new ShoppingList4.Domain.Entities.Entry { Id = 1, Name = "NewName" };

            _entryServiceMock
                .Setup(s => s.Update(It.IsAny<EditEntryDto>()))
                .ReturnsAsync(updatedEntry);

            // Act
            await _viewModel.EditCommand.ExecuteAsync(entry);

            // Assert
            _viewModel.Entries.Should().ContainSingle(e => e.Name == "NewName");
        }

        [Fact]
        public async Task DeleteAllCommand_ShouldRemoveAllEntries_WhenConfirmed()
        {
            // Arrange
            var entries = new List<EntryViewModel>
            {
                new(new ShoppingList4.Domain.Entities.Entry { Id = 1, Name = "Eggs" }),
                new(new ShoppingList4.Domain.Entities.Entry { Id = 2, Name = "Milk" })
            };

            _viewModel.Entries = new ObservableCollection<EntryViewModel>(entries);

            _messageBoxServiceMock
                .Setup(m => m.ShowAlert("Potwierdzenie", "Czy na pewno chcesz usunąć wszystkie pozycje?", "TAK", "NIE"))
                .ReturnsAsync(true);

            _entryServiceMock
                .Setup(s => s.DeleteMultiple(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(true);

            // Act
            await _viewModel.DeleteAllCommand.ExecuteAsync(null);

            // Assert
            _viewModel.Entries.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteBoughtCommand_ShouldRemoveOnlyBoughtEntries_WhenConfirmed()
        {
            // Arrange
            var entries = new List<EntryViewModel>
            {
                new(new ShoppingList4.Domain.Entities.Entry { Id = 1, Name = "Eggs", IsBought = true }),
                new(new ShoppingList4.Domain.Entities.Entry { Id = 2, Name = "Milk", IsBought = false })
            };

            _viewModel.Entries = new ObservableCollection<EntryViewModel>(entries);

            _messageBoxServiceMock
                .Setup(m => m.ShowAlert("Potwierdzenie", "Czy na pewno chcesz usunąć kupione pozycje?", "TAK", "NIE"))
                .ReturnsAsync(true);

            _entryServiceMock
                .Setup(s => s.DeleteMultiple(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(true);

            // Act
            await _viewModel.DeleteBoughtCommand.ExecuteAsync(null);

            // Assert
            _viewModel.Entries.Should().ContainSingle();
            _viewModel.Entries.Should().Contain(e => e.Name == "Milk");
        }
    }
}