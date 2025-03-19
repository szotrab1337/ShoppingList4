using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ShoppingList4.Domain.Entities;
using ShoppingList4.Domain.Interfaces;
using ShoppingList4.Maui.Dtos;
using ShoppingList4.Maui.Interfaces;
using ShoppingList4.Maui.Services;
using Entry = ShoppingList4.Domain.Entities.Entry;

namespace ShoppingList4.Maui.Tests.Services
{
    public class EntryServiceTests
    {
        private readonly Mock<IEntryRepository> _entryRepositoryMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly EntryService _entryService;

        public EntryServiceTests()
        {
            _entryRepositoryMock = new Mock<IEntryRepository>();
            _userServiceMock = new Mock<IUserService>();
            Mock<ILogger<EntryService>> loggerMock = new();

            _entryService = new EntryService(
                _entryRepositoryMock.Object,
                _userServiceMock.Object,
                loggerMock.Object);
        }

        [Fact]
        public async Task GetShoppingListEntries_ShouldReturnEntries_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 1, Email = "email@email.com", Name = "name", ApiToken = "token123" };
            var entries = new List<Entry>
            {
                new() { Id = 1, Name = "Milk", ShoppingListId = 1 },
                new() { Id = 2, Name = "Bread", ShoppingListId = 1 }
            };

            _userServiceMock.Setup(x => x.GetCurrentUser()).ReturnsAsync(user);
            _entryRepositoryMock.Setup(x => x.GetByShoppingListId(user.ApiToken, 1, null))
                .ReturnsAsync(entries);

            // Act
            var result = await _entryService.GetShoppingListEntries(1);

            // Assert
            result.Should().BeEquivalentTo(entries);
        }

        [Fact]
        public async Task GetShoppingListEntries_ShouldReturnEmptyList_WhenUserIsNull()
        {
            // Arrange
            _userServiceMock.Setup(x => x.GetCurrentUser()).ReturnsAsync((User?)null);

            // Act
            var result = await _entryService.GetShoppingListEntries(1);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Add_ShouldReturnAddedEntry_WhenSuccess()
        {
            // Arrange
            var user = new User { Id = 1, Email = "email@email.com", Name = "name", ApiToken = "token123" };
            var dto = new AddEntryDto { Name = "Eggs" };
            var addedEntry = new Entry { Id = 3, Name = "Eggs" };

            _userServiceMock.Setup(x => x.GetCurrentUser()).ReturnsAsync(user);
            _entryRepositoryMock.Setup(x => x.Add(user.ApiToken, dto, null)).ReturnsAsync((true, addedEntry));

            // Act
            var result = await _entryService.Add(dto);

            // Assert
            result.Should().BeEquivalentTo(addedEntry);
        }

        [Fact]
        public async Task Update_ShouldReturnUpdatedEntry_WhenSuccess()
        {
            // Arrange
            var user = new User { Id = 1, Email = "email@email.com", Name = "name", ApiToken = "token123" };
            var dto = new EditEntryDto { Id = 3, Name = "Cheese" };
            var updatedEntry = new Entry { Id = 3, Name = "Cheese" };

            _userServiceMock.Setup(x => x.GetCurrentUser()).ReturnsAsync(user);
            _entryRepositoryMock.Setup(x => x.Edit(user.ApiToken, dto, null)).ReturnsAsync((true, updatedEntry));

            // Act
            var result = await _entryService.Update(dto);

            // Assert
            result.Should().BeEquivalentTo(updatedEntry);
        }

        [Fact]
        public async Task Delete_ShouldReturnTrue_WhenEntryIsDeleted()
        {
            // Arrange
            var user = new User { Id = 1, Email = "email@email.com", Name = "name", ApiToken = "token123" };
            const int entryId = 3;

            _userServiceMock.Setup(x => x.GetCurrentUser()).ReturnsAsync(user);
            _entryRepositoryMock.Setup(x => x.Delete(user.ApiToken, entryId, null)).ReturnsAsync(true);

            // Act
            var result = await _entryService.Delete(entryId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Delete_ShouldReturnFalse_WhenEntryDeletionFails()
        {
            // Arrange
            var user = new User { Id = 1, Email = "email@email.com", Name = "name", ApiToken = "token123" };
            const int entryId = 3;

            _userServiceMock.Setup(x => x.GetCurrentUser()).ReturnsAsync(user);
            _entryRepositoryMock.Setup(x => x.Delete(user.ApiToken, entryId, null)).ReturnsAsync(false);

            // Act
            var result = await _entryService.Delete(entryId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteMultiple_ShouldReturnTrue_WhenEntriesDeleted()
        {
            // Arrange
            var user = new User { Id = 1, Email = "email@email.com", Name = "name", ApiToken = "token123" };
            var entryIds = new List<int> { 1, 2, 3 };

            _userServiceMock.Setup(x => x.GetCurrentUser()).ReturnsAsync(user);
            _entryRepositoryMock.Setup(x => x.DeleteMultiple(user.ApiToken, entryIds.ToArray(), null))
                .ReturnsAsync(true);

            // Act
            var result = await _entryService.DeleteMultiple(entryIds);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteMultiple_ShouldReturnFalse_WhenEntriesDeletionFails()
        {
            // Arrange
            var user = new User { Id = 1, Email = "email@email.com", Name = "name", ApiToken = "token123" };
            var entryIds = new List<int> { 1, 2, 3 };

            _userServiceMock.Setup(x => x.GetCurrentUser()).ReturnsAsync(user);
            _entryRepositoryMock.Setup(x => x.DeleteMultiple(user.ApiToken, entryIds.ToArray(), null))
                .ReturnsAsync(false);

            // Act
            var result = await _entryService.DeleteMultiple(entryIds);

            // Assert
            result.Should().BeFalse();
        }
    }
}