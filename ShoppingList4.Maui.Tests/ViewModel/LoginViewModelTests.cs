using FluentAssertions;
using Moq;
using ShoppingList4.Application.Interfaces;
using ShoppingList4.Maui.Interfaces;
using ShoppingList4.Maui.ViewModel;

namespace ShoppingList4.Maui.Tests.ViewModel
{
    public class LoginViewModelTests
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly Mock<IMessageBoxService> _messageBoxServiceMock;
        private readonly Mock<INavigationService> _navigationServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly LoginViewModel _viewModel;

        public LoginViewModelTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _userServiceMock = new Mock<IUserService>();
            _messageBoxServiceMock = new Mock<IMessageBoxService>();
            _navigationServiceMock = new Mock<INavigationService>();

            _viewModel = new LoginViewModel(
                _accountServiceMock.Object,
                _userServiceMock.Object,
                _messageBoxServiceMock.Object,
                _navigationServiceMock.Object
            );
        }

        [Fact]
        public async Task Initialize_ShouldSetUserExists()
        {
            // Arrange
            _userServiceMock.Setup(s => s.ExistsCurrentUser()).ReturnsAsync(true);

            // Act
            await _viewModel.Initialize();

            // Assert
            _viewModel.UserExists.Should().BeTrue();
            _viewModel.Email.Should().BeEmpty();
            _viewModel.Password.Should().BeEmpty();
        }

        [Fact]
        public async Task LoginCommand_ShouldExecuteSuccessfully_WhenCredentialsAreValid()
        {
            // Arrange
            _viewModel.Email = "test@example.com";
            _viewModel.Password = "password123";

            _accountServiceMock.Setup(a => a.Login(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            _userServiceMock.Setup(u => u.ExistsCurrentUser()).ReturnsAsync(true);

            // Act
            await _viewModel.LoginCommand.ExecuteAsync(null);

            // Assert
            _navigationServiceMock.Verify(n => n.NavigateTo("//Main"), Times.Once);
            _messageBoxServiceMock.Verify(m => m.ShowAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task LoginCommand_ShouldShowError_OnException()
        {
            // Arrange
            _viewModel.Email = "test@example.com";
            _viewModel.Password = "password123";

            _accountServiceMock.Setup(a => a.Login(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            await _viewModel.LoginCommand.ExecuteAsync(null);

            // Assert
            _navigationServiceMock.Verify(n => n.NavigateTo(It.IsAny<string>()), Times.Never);
            _messageBoxServiceMock.Verify(m => m.ShowAlert("Błąd", "Wystąpił błąd. Spróbuj ponownie.", "OK"),
                Times.Once);
        }

        [Fact]
        public async Task LoginCommand_ShouldShowError_WhenUserDoesNotExist()
        {
            // Arrange
            _viewModel.Email = "test@example.com";
            _viewModel.Password = "password123";

            _accountServiceMock.Setup(a => a.Login(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            _userServiceMock.Setup(u => u.ExistsCurrentUser()).ReturnsAsync(false);

            // Act
            await _viewModel.LoginCommand.ExecuteAsync(null);

            // Assert
            _messageBoxServiceMock.Verify(m => m.ShowAlert("Błąd", "Niepoprawne dane.", "OK"), Times.Once);
            _navigationServiceMock.Verify(n => n.NavigateTo(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void LogoutCommand_ShouldClearUserData()
        {
            // Arrange
            _viewModel.UserExists = true;

            // Act
            _viewModel.LogoutCommand.Execute(null);

            // Assert
            _viewModel.UserExists.Should().BeFalse();
            _userServiceMock.Verify(u => u.RemoveCurrentUser(), Times.Once);
        }
    }
}