using FluentAssertions;
using ShoppingList4.Domain.Entities;
using ShoppingList4.Maui.ViewModel.Entities;

namespace ShoppingList4.Maui.Tests.ViewModel.Entities
{
    public class ShoppingListViewModelTests
    {
        [Fact]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var shoppingList = new ShoppingList { Id = 1, Name = "Groceries", ItemsCount = 5, ItemsBoughtCount = 2 };

            // Act
            var viewModel = new ShoppingListViewModel(shoppingList);

            // Assert
            viewModel.Id.Should().Be(1);
            viewModel.Name.Should().Be("Groceries");
            viewModel.ItemsCount.Should().Be(5);
            viewModel.ItemsBoughtCount.Should().Be(2);
        }

        [Fact]
        public void Update_ShouldUpdatePropertiesCorrectly()
        {
            // Arrange
            var shoppingList = new ShoppingList { Id = 1, Name = "Groceries", ItemsCount = 5, ItemsBoughtCount = 2 };

            var viewModel = new ShoppingListViewModel(shoppingList);

            var updatedShoppingList = new ShoppingList
            {
                Id = 1,
                Name = "Electronics",
                ItemsCount = 10,
                ItemsBoughtCount = 7
            };

            // Act
            viewModel.Update(updatedShoppingList);

            // Assert
            viewModel.Name.Should().Be("Electronics");
            viewModel.ItemsCount.Should().Be(10);
            viewModel.ItemsBoughtCount.Should().Be(7);
        }
    }
}