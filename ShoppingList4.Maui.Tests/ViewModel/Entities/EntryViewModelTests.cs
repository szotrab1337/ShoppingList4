using CommunityToolkit.Mvvm.Input;
using FluentAssertions;
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
            var viewModel = new EntryViewModel(entry);

            // Assert
            viewModel.Id.Should().Be(1);
            viewModel.Name.Should().Be("Milk");
            viewModel.IsBought.Should().BeFalse();
            viewModel.ShoppingListId.Should().Be(10);
        }

        [Fact]
        public void Update_ShouldUpdatePropertiesCorrectly()
        {
            // Arrange
            var entry = new Entry { Name = "Milk", IsBought = false };
            var viewModel = new EntryViewModel(entry, null);

            var updatedEntry = new Entry { Name = "Bread", IsBought = true };

            // Act
            viewModel.Update(updatedEntry);

            // Assert
            viewModel.Name.Should().Be("Bread");
            viewModel.IsBought.Should().BeTrue();
        }

        [Fact]
        public void OnIsBoughtChanged_ShouldTriggerEvent()
        {
            // Arrange
            var entry = new Entry { Name = "Milk", IsBought = false };
            var viewModel = new EntryViewModel(entry);
            bool eventRaised = false;

            viewModel.OnBoughtStatusChanged += (_, _) => eventRaised = true;

            // Act
            viewModel.IsBought = true; // should execute OnIsBoughtChanged

            // Assert
            eventRaised.Should().BeTrue();
        }
    }
}