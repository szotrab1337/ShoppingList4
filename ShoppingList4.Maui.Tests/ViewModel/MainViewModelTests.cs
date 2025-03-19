using CommunityToolkit.Maui.Views;
using FluentAssertions;
using Moq;
using ShoppingList4.Domain.Entities;
using ShoppingList4.Maui.Dtos;
using ShoppingList4.Maui.Interfaces;
using ShoppingList4.Maui.ViewModel;
using ShoppingList4.Maui.ViewModel.Entities;

namespace ShoppingList4.Maui.Tests.ViewModel
{
    public class MainViewModelTests
    {
        private readonly Mock<IUserService> _userServiceMock = new();
        private readonly Mock<IShoppingListService> _shoppingListServiceMock = new();
        private readonly Mock<IMessageBoxService> _messageBoxServiceMock = new();
        private readonly Mock<IDialogService> _dialogServiceMock = new();
        private readonly Mock<INavigationService> _navigationServiceMock = new();

        private readonly MainViewModel _viewModel;

        public MainViewModelTests()
        {
            _viewModel = new MainViewModel(
                _userServiceMock.Object,
                _shoppingListServiceMock.Object,
                _messageBoxServiceMock.Object,
                _dialogServiceMock.Object,
                _navigationServiceMock.Object);
        }

        [Fact]
        public async Task Initialize_ShouldSetIsInitializingAndLoadShoppingLists_WhenUserExists()
        {
            // Arrange
            _userServiceMock.Setup(x => x.ExistsCurrentUser()).ReturnsAsync(true);
            _shoppingListServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<ShoppingList>
            {
                new() { Id = 1, Name = "Lista 1" }
            });

            // Act
            await _viewModel.Initialize();

            // Assert
            _viewModel.IsInitializing.Should().BeFalse();
            _viewModel.ShoppingLists.Should().HaveCount(1);
            _viewModel.ShoppingLists.First().Id.Should().Be(1);
            _viewModel.ShoppingLists.First().Name.Should().Be("Lista 1");
            _shoppingListServiceMock.Verify(x => x.GetAll(), Times.Once);
        }

        [Fact]
        public async Task Initialize_ShouldNotLoadShoppingLists_WhenUserDoesNotExist()
        {
            // Arrange
            _userServiceMock.Setup(x => x.ExistsCurrentUser()).ReturnsAsync(false);

            // Act
            await _viewModel.Initialize();

            // Assert
            _shoppingListServiceMock.Verify(x => x.GetAll(), Times.Never);
            _navigationServiceMock.Verify(x => x.NavigateTo("//Login"));
        }

        [Fact]
        public async Task RefreshCommand_ShouldReloadShoppingLists()
        {
            // Arrange
            _shoppingListServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<ShoppingList>
            {
                new() { Id = 1, Name = "Lista 1" }
            });

            // Act
            await _viewModel.RefreshCommand.ExecuteAsync(null);

            // Assert
            _viewModel.ShoppingLists.Should().HaveCount(1);
            _viewModel.ShoppingLists.First().Id.Should().Be(1);
            _viewModel.ShoppingLists.First().Name.Should().Be("Lista 1");
        }

        [Fact]
        public async Task DeleteCommand_ShouldRemoveShoppingList_WhenConfirmed()
        {
            // Arrange
            var shoppingList = new ShoppingListViewModel(new ShoppingList { Id = 1, Name = "Lista 1" });
            _viewModel.ShoppingLists = [shoppingList];

            _messageBoxServiceMock.Setup(x => x.ShowAlert(It.IsAny<string>(), It.IsAny<string>(), "TAK", "NIE"))
                .ReturnsAsync(true);
            _shoppingListServiceMock.Setup(x => x.Delete(shoppingList.Id)).ReturnsAsync(true);

            // Act
            await _viewModel.DeleteCommand.ExecuteAsync(shoppingList);

            // Assert
            _viewModel.ShoppingLists.Should().BeEmpty();
            _shoppingListServiceMock.Verify(x => x.Delete(shoppingList.Id), Times.Once);
        }

        [Fact]
        public async Task DeleteCommand_ShouldNotRemoveShoppingList_WhenCancelled()
        {
            // Arrange
            var shoppingList = new ShoppingListViewModel(new ShoppingList { Id = 1, Name = "Lista 1" });
            _viewModel.ShoppingLists = [shoppingList];

            _messageBoxServiceMock.Setup(x => x.ShowAlert(It.IsAny<string>(), It.IsAny<string>(), "TAK", "NIE"))
                .ReturnsAsync(false);

            // Act
            await _viewModel.DeleteCommand.ExecuteAsync(shoppingList);

            // Assert
            _viewModel.ShoppingLists.Should().HaveCount(1);
            _shoppingListServiceMock.Verify(x => x.Delete(shoppingList.Id), Times.Never);
        }

        [Fact]
        public async Task AddCommand_ShouldAddShoppingList_WhenUserProvidesName()
        {
            // Arrange
            _dialogServiceMock.Setup(x => x.ShowPromptAsync(It.IsAny<Popup>()))
                .ReturnsAsync("Nowa Lista");

            _shoppingListServiceMock.Setup(x => x.Add(It.IsAny<AddShoppingListDto>()))
                .ReturnsAsync(new ShoppingList { Id = 2, Name = "Nowa Lista" });

            // Act
            await _viewModel.AddCommand.ExecuteAsync(null);

            // Assert
            _viewModel.ShoppingLists.Should().Contain(x => x.Name == "Nowa Lista" && x.Id == 2);
            _shoppingListServiceMock.Verify(x => x.Add(It.IsAny<AddShoppingListDto>()), Times.Once);
        }

        [Fact]
        public async Task AddCommand_ShouldNotAddShoppingList_WhenCancelled()
        {
            // Arrange
            _dialogServiceMock.Setup(x => x.ShowPromptAsync(It.IsAny<Popup>()))
                .ReturnsAsync(string.Empty);

            // Act
            await _viewModel.AddCommand.ExecuteAsync(null);

            // Assert
            _shoppingListServiceMock.Verify(x => x.Add(It.IsAny<AddShoppingListDto>()), Times.Never);
        }

        [Fact]
        public async Task EditCommand_ShouldUpdateShoppingList_WhenUserProvidesNewName()
        {
            // Arrange
            var shoppingList = new ShoppingListViewModel(new ShoppingList { Id = 1, Name = "Lista 1" });
            _viewModel.ShoppingLists = [shoppingList];

            _dialogServiceMock.Setup(x => x.ShowPromptAsync(It.IsAny<Popup>()))
                .ReturnsAsync("Zmieniona Lista");

            _shoppingListServiceMock.Setup(x => x.Update(It.IsAny<EditShoppingListDto>()))
                .ReturnsAsync(new ShoppingList { Id = 1, Name = "Zmieniona Lista" });

            // Act
            await _viewModel.EditCommand.ExecuteAsync(shoppingList);

            // Assert
            _viewModel.ShoppingLists.Should().ContainSingle(x => x.Name == "Zmieniona Lista" && x.Id == 1);
            _shoppingListServiceMock.Verify(x => x.Update(It.IsAny<EditShoppingListDto>()), Times.Once);
        }

        [Fact]
        public async Task EditCommand_ShouldNotUpdateShoppingList_WhenCancelled()
        {
            // Arrange
            var shoppingList = new ShoppingListViewModel(new ShoppingList { Id = 1, Name = "Lista 1" });
            _viewModel.ShoppingLists = [shoppingList];

            _dialogServiceMock.Setup(x => x.ShowPromptAsync(It.IsAny<Popup>()))
                .ReturnsAsync(string.Empty);

            // Act
            await _viewModel.EditCommand.ExecuteAsync(shoppingList);

            // Assert
            _viewModel.ShoppingLists.Should().ContainSingle(x => x.Name == "Lista 1" && x.Id == 1);
            _shoppingListServiceMock.Verify(x => x.Update(It.IsAny<EditShoppingListDto>()), Times.Never);
        }

        [Fact]
        public async Task OpenCommand_ShouldNavigateToEntriesPage()
        {
            // Arrange
            var shoppingList = new ShoppingListViewModel(new ShoppingList { Id = 1, Name = "Lista 1" });

            // Act
            await _viewModel.OpenCommand.ExecuteAsync(shoppingList);

            // Assert
            _navigationServiceMock.Verify(x =>
                x.NavigateTo(It.IsAny<ShellNavigationState>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
        }
    }
}