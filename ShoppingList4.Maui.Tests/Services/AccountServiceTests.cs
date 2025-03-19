using Moq;
using ShoppingList4.Domain.Entities;
using ShoppingList4.Domain.Interfaces;
using ShoppingList4.Maui.Interfaces;
using ShoppingList4.Maui.Services;

namespace ShoppingList4.Maui.Tests.Services
{
    public class AccountServiceTests
    {
        private readonly Mock<IUsersRepository> _usersRepositoryMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly AccountService _accountService;

        public AccountServiceTests()
        {
            _usersRepositoryMock = new Mock<IUsersRepository>();
            _userServiceMock = new Mock<IUserService>();
            _accountService = new AccountService(_usersRepositoryMock.Object, _userServiceMock.Object);
        }

        [Fact]
        public async Task Login_ShouldSetCurrentUser_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 1, Email = "test@example.com", Name = "Test", ApiToken = "ABC" };
            _usersRepositoryMock.Setup(x => x.Login("test@example.com", "password", null))
                .ReturnsAsync(user);

            // Act
            await _accountService.Login("test@example.com", "password");

            // Assert
            _userServiceMock.Verify(x => x.SetCurrentUser(user), Times.Once);
            _userServiceMock.Verify(x => x.RemoveCurrentUser(), Times.Never);
        }

        [Fact]
        public async Task Login_ShouldRemoveCurrentUser_WhenUserDoesNotExist()
        {
            // Arrange
            _usersRepositoryMock.Setup(x => x.Login("test@example.com", "wrong_password", null))
                .ReturnsAsync((User?)null);

            // Act
            await _accountService.Login("test@example.com", "wrong_password");

            // Assert
            _userServiceMock.Verify(x => x.RemoveCurrentUser(), Times.Once);
            _userServiceMock.Verify(x => x.SetCurrentUser(It.IsAny<User>()), Times.Never);
        }
    }
}