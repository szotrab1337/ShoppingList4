using CommunityToolkit.Mvvm.Input;
using FluentAssertions;
using NSubstitute;
using ShoppingList4.Maui.ViewModel.Entities;
using Entry = ShoppingList4.Domain.Entities.Entry;

namespace ShoppingList4.Maui.Tests.ViewModel.Entities
{
    public class EntryViewModelTests
    {
        [Fact]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var entry = new Entry
            {
                Id = 1,
                Name = "Milk",
                IsBought = false,
                ShoppingListId = 10
            };

            // Act
            var viewModel = new EntryViewModel(entry, null!);

            // Assert
            viewModel.Id.Should().Be(1);
            viewModel.Name.Should().Be("Milk");
            viewModel.IsBought.Should().BeFalse();
            viewModel.ShoppingListId.Should().Be(10);
        }

        [Fact]
        public void OnIsBoughtChanged_ShouldExecuteCommand_WhenCommandIsProvided()
        {
            // Arrange
            var entry = new Entry { Name = "Milk", IsBought = false };
            var mockCommand = Substitute.For<IRelayCommand<EntryViewModel>>();
            var viewModel = new EntryViewModel(entry, mockCommand);

            // Act
            viewModel.IsBought = true;

            // Assert
            mockCommand.Received(1).Execute(viewModel);
        }

        [Fact]
        public void OnIsBoughtChanged_ShouldNotThrow_WhenCommandIsNull()
        {
            // Arrange
            var entry = new Entry { Name = "Milk", IsBought = false };
            var viewModel = new EntryViewModel(entry, null);

            // Act
            Action act = () => viewModel.IsBought = true;

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Update_ShouldUpdatePropertiesCorrectly()
        {
            // Arrange
            var entry = new Entry { Name = "Milk", IsBought = false };
            var viewModel = new EntryViewModel(entry, null!);

            var updatedEntry = new Entry { Name = "Bread", IsBought = true };

            // Act
            viewModel.Update(updatedEntry);

            // Assert
            viewModel.Name.Should().Be("Bread");
            viewModel.IsBought.Should().BeTrue();
        }
    }
}