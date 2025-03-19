using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ShoppingList4.Domain.Entities;
using ShoppingList4.Domain.Interfaces;
using ShoppingList4.Maui.Dtos;
using ShoppingList4.Maui.Interfaces;
using ShoppingList4.Maui.Services;

namespace ShoppingList4.Maui.Tests.Services
{
    public class ShoppingListServiceTests
    {
        private readonly Mock<IShoppingListRepository> _shoppingListRepositoryMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly ShoppingListService _shoppingListService;

        public ShoppingListServiceTests()
        {
            _shoppingListRepositoryMock = new Mock<IShoppingListRepository>();
            _userServiceMock = new Mock<IUserService>();
            Mock<ILogger<ShoppingListService>> loggerMock = new();

            _shoppingListService = new ShoppingListService(
                _shoppingListRepositoryMock.Object,
                _userServiceMock.Object,
                loggerMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnShoppingLists_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 1, Email = "email@email.com", Name = "name", ApiToken = "token123" };
            var shoppingLists = new List<ShoppingList>
            {
                new() { Id = 1, Name = "Groceries" }, new() { Id = 2, Name = "Electronics" }
            };

            _userServiceMock.Setup(x => x.GetCurrentUser()).ReturnsAsync(user);
            _shoppingListRepositoryMock.Setup(x => x.GetAll(user.ApiToken, null)).ReturnsAsync(shoppingLists);

            // Act
            var result = await _shoppingListService.GetAll();

            // Assert
            result.Should().BeEquivalentTo(shoppingLists);
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmptyList_WhenUserIsNull()
        {
            // Arrange
            _userServiceMock.Setup(x => x.GetCurrentUser()).ReturnsAsync((User?)null);

            // Act
            var result = await _shoppingListService.GetAll();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Add_ShouldReturnAddedShoppingList_WhenSuccess()
        {
            // Arrange
            var user = new User { Id = 1, Email = "email@email.com", Name = "name", ApiToken = "token123" };
            var dto = new AddShoppingListDto { Name = "Household Items" };
            var addedShoppingList = new ShoppingList { Id = 3, Name = "Household Items" };

            _userServiceMock.Setup(x => x.GetCurrentUser()).ReturnsAsync(user);
            _shoppingListRepositoryMock.Setup(x => x.Add(user.ApiToken, dto, null))
                .ReturnsAsync((true, addedShoppingList));

            // Act
            var result = await _shoppingListService.Add(dto);

            // Assert
            result.Should().BeEquivalentTo(addedShoppingList);
        }

        [Fact]
        public async Task Update_ShouldReturnUpdatedShoppingList_WhenSuccess()
        {
            // Arrange
            var user = new User { Id = 1, Email = "email@email.com", Name = "name", ApiToken = "token123" };
            var dto = new EditShoppingListDto { Id = 3, Name = "Updated List" };
            var updatedShoppingList = new ShoppingList { Id = 3, Name = "Updated List" };

            _userServiceMock.Setup(x => x.GetCurrentUser()).ReturnsAsync(user);
            _shoppingListRepositoryMock.Setup(x => x.Edit(user.ApiToken, dto, null))
                .ReturnsAsync((true, updatedShoppingList));

            // Act
            var result = await _shoppingListService.Update(dto);

            // Assert
            result.Should().BeEquivalentTo(updatedShoppingList);
        }

        [Fact]
        public async Task Delete_ShouldReturnTrue_WhenShoppingListIsDeleted()
        {
            // Arrange
            var user = new User { Id = 1, Email = "email@email.com", Name = "name", ApiToken = "token123" };
            const int shoppingListId = 3;

            _userServiceMock.Setup(x => x.GetCurrentUser()).ReturnsAsync(user);
            _shoppingListRepositoryMock.Setup(x => x.Delete(user.ApiToken, shoppingListId, null)).ReturnsAsync(true);

            // Act
            var result = await _shoppingListService.Delete(shoppingListId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Delete_ShouldReturnFalse_WhenShoppingListDeletionFails()
        {
            // Arrange
            var user = new User { Id = 1, Email = "email@email.com", Name = "name", ApiToken = "token123" };
            const int shoppingListId = 3;

            _userServiceMock.Setup(x => x.GetCurrentUser()).ReturnsAsync(user);
            _shoppingListRepositoryMock.Setup(x => x.Delete(user.ApiToken, shoppingListId, null)).ReturnsAsync(false);

            // Act
            var result = await _shoppingListService.Delete(shoppingListId);

            // Assert
            result.Should().BeFalse();
        }
    }
}