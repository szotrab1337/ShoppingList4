using FluentAssertions;
using Moq;
using ShoppingList4.Application.Dtos;
using ShoppingList4.Application.Interfaces;
using ShoppingList4.Domain.Entities;
using ShoppingList4.Maui.Interfaces;
using ShoppingList4.Maui.ViewModel;
using ShoppingList4.Maui.ViewModel.Entities;

namespace ShoppingList4.Maui.Tests.ViewModel
{
    public class MainViewModelTests
    {
        private readonly Mock<IDialogService> _dialogServiceMock;
        private readonly Mock<IMessageBoxService> _messageBoxServiceMock;
        private readonly Mock<INavigationService> _navigationServiceMock;
        private readonly Mock<IShoppingListService> _shoppingListServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly MainViewModel _viewModel;

        public MainViewModelTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _shoppingListServiceMock = new Mock<IShoppingListService>();
            _messageBoxServiceMock = new Mock<IMessageBoxService>();
            _dialogServiceMock = new Mock<IDialogService>();
            _navigationServiceMock = new Mock<INavigationService>();

            _viewModel = new MainViewModel(
                _userServiceMock.Object,
                _shoppingListServiceMock.Object,
                _messageBoxServiceMock.Object,
                _dialogServiceMock.Object,
                _navigationServiceMock.Object
            );
        }

        [Fact]
        public async Task AddCommand_ShouldAddShoppingList_WhenNameIsProvided()
        {
            // Arrange
            _dialogServiceMock.Setup(d => d.ShowInputPopup(null)).ReturnsAsync("Nowa lista");

            // Act
            await _viewModel.AddCommand.ExecuteAsync(null);

            // Assert
            _shoppingListServiceMock.Verify(s => s.Add(It.Is<AddShoppingListDto>(dto =>
                dto.Name == "Nowa lista")), Times.Once);
        }

        [Fact]
        public async Task AddCommand_ShouldNotAddShoppingList_WhenNameIsEmpty()
        {
            // Arrange
            _dialogServiceMock.Setup(d => d.ShowInputPopup(null)).ReturnsAsync(string.Empty);

            // Act
            await _viewModel.AddCommand.ExecuteAsync(null);

            // Assert
            _shoppingListServiceMock.Verify(s => s.Add(It.IsAny<AddShoppingListDto>()), Times.Never);
        }

        [Fact]
        public async Task DeleteCommand_ShouldDeleteShoppingList_WhenConfirmed()
        {
            // Arrange
            var shoppingList = new ShoppingList { Id = 1, Name = "Lista testowa" };
            var shoppingListViewModel = new ShoppingListViewModel(shoppingList);

            _messageBoxServiceMock.Setup(m => m.ShowAlert(
                    "Potwierdzenie", "Czy na pewno chcesz usunąć wybraną listę?", "TAK", "NIE"))
                .ReturnsAsync(true);

            // Act
            await _viewModel.DeleteCommand.ExecuteAsync(shoppingListViewModel);

            // Assert
            _shoppingListServiceMock.Verify(s => s.Delete(1), Times.Once);
        }

        [Fact]
        public async Task DeleteCommand_ShouldNotDelete_WhenNotConfirmed()
        {
            // Arrange
            var shoppingList = new ShoppingList { Id = 1, Name = "Lista testowa" };
            var shoppingListViewModel = new ShoppingListViewModel(shoppingList);

            _messageBoxServiceMock.Setup(m => m.ShowAlert(
                    "Potwierdzenie", "Czy na pewno chcesz usunąć wybraną listę?", "TAK", "NIE"))
                .ReturnsAsync(false);

            // Act
            await _viewModel.DeleteCommand.ExecuteAsync(shoppingListViewModel);

            // Assert
            _shoppingListServiceMock.Verify(s => s.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteCommand_ShouldShowError_OnException()
        {
            // Arrange
            var shoppingList = new ShoppingList { Id = 1, Name = "Lista testowa" };
            var shoppingListViewModel = new ShoppingListViewModel(shoppingList);

            _messageBoxServiceMock.Setup(m => m.ShowAlert(
                    "Potwierdzenie", "Czy na pewno chcesz usunąć wybraną listę?", "TAK", "NIE"))
                .ReturnsAsync(true);

            _shoppingListServiceMock.Setup(s => s.Delete(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            await _viewModel.DeleteCommand.ExecuteAsync(shoppingListViewModel);

            // Assert
            _messageBoxServiceMock.Verify(m => m.ShowAlert("Błąd", "Wystąpił błąd. Spróbuj ponownie.", "OK"),
                Times.Once);
        }

        [Fact]
        public async Task EditCommand_ShouldNotUpdateShoppingList_WhenNameIsEmpty()
        {
            // Arrange
            var shoppingList = new ShoppingList { Id = 1, Name = "Stara nazwa" };
            var shoppingListViewModel = new ShoppingListViewModel(shoppingList);

            _dialogServiceMock.Setup(d => d.ShowInputPopup(It.IsAny<string>())).ReturnsAsync(string.Empty);

            // Act
            await _viewModel.EditCommand.ExecuteAsync(shoppingListViewModel);

            // Assert
            _shoppingListServiceMock.Verify(s => s.Update(It.IsAny<EditShoppingListDto>()), Times.Never);
        }

        [Fact]
        public async Task EditCommand_ShouldUpdateShoppingList_WhenNameIsProvided()
        {
            // Arrange
            var shoppingList = new ShoppingList { Id = 1, Name = "Stara nazwa" };
            var shoppingListViewModel = new ShoppingListViewModel(shoppingList);

            _dialogServiceMock.Setup(d => d.ShowInputPopup("Stara nazwa")).ReturnsAsync("Nowa nazwa");

            // Act
            await _viewModel.EditCommand.ExecuteAsync(shoppingListViewModel);

            // Assert
            _shoppingListServiceMock.Verify(s => s.Update(It.Is<EditShoppingListDto>(dto =>
                dto.Id == 1 && dto.Name == "Nowa nazwa")), Times.Once);
        }

        [Fact]
        public async Task Initialize_ShouldLoadShoppingLists_WhenUserExists()
        {
            // Arrange
            var shoppingLists = new List<ShoppingList>
            {
                new() { Id = 1, Name = "Zakupy spożywcze", ItemsCount = 5, ItemsBoughtCount = 2 },
                new() { Id = 2, Name = "Zakupy chemiczne", ItemsCount = 3, ItemsBoughtCount = 1 }
            };

            _userServiceMock.Setup(s => s.ExistsCurrentUser()).ReturnsAsync(true);
            _shoppingListServiceMock.Setup(s => s.GetAll()).ReturnsAsync(shoppingLists);

            // Act
            await _viewModel.Initialize();

            // Assert
            _viewModel.ShoppingLists.Should().HaveCount(2);
            _viewModel.IsInitializing.Should().BeFalse();
            _navigationServiceMock.Verify(n => n.NavigateTo(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Initialize_ShouldNavigateToLogin_WhenUserDoesNotExist()
        {
            // Arrange
            _userServiceMock.Setup(s => s.ExistsCurrentUser()).ReturnsAsync(false);

            // Act
            await _viewModel.Initialize();

            // Assert
            _navigationServiceMock.Verify(n => n.NavigateTo("//Login"), Times.Once);
            _shoppingListServiceMock.Verify(s => s.GetAll(), Times.Never);
        }

        [Fact]
        public async Task Initialize_ShouldNotLoadAgain_WhenAlreadyLoaded()
        {
            // Arrange
            _userServiceMock.Setup(s => s.ExistsCurrentUser()).ReturnsAsync(true);
            _shoppingListServiceMock.Setup(s => s.GetAll()).ReturnsAsync(new List<ShoppingList>());

            await _viewModel.Initialize();

            // Act
            await _viewModel.Initialize();

            // Assert
            _userServiceMock.Verify(s => s.ExistsCurrentUser(), Times.Once);
            _shoppingListServiceMock.Verify(s => s.GetAll(), Times.Once);
        }

        [Fact]
        public async Task RefreshCommand_ShouldReloadShoppingLists()
        {
            // Arrange
            var shoppingLists = new List<ShoppingList>
            {
                new() { Id = 1, Name = "Lista 1" }
            };

            _shoppingListServiceMock.Setup(s => s.GetAll()).ReturnsAsync(shoppingLists);

            // Act
            await _viewModel.RefreshCommand.ExecuteAsync(null);

            // Assert
            _viewModel.IsRefreshing.Should().BeFalse();
            _shoppingListServiceMock.Verify(s => s.GetAll(), Times.Once);
        }

        [Fact]
        public async Task RefreshCommand_ShouldShowError_OnException()
        {
            // Arrange
            _shoppingListServiceMock.Setup(s => s.GetAll())
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            await _viewModel.RefreshCommand.ExecuteAsync(null);

            // Assert
            _messageBoxServiceMock.Verify(m => m.ShowAlert("Błąd", "Wystąpił błąd. Spróbuj ponownie.", "OK"),
                Times.Once);
            _viewModel.IsRefreshing.Should().BeFalse();
        }

        [Fact]
        public async Task ShoppingListAdded_Event_ShouldAddToCollection()
        {
            // Arrange
            _userServiceMock.Setup(s => s.ExistsCurrentUser()).ReturnsAsync(true);
            _shoppingListServiceMock.Setup(s => s.GetAll()).ReturnsAsync(new List<ShoppingList>());

            await _viewModel.Initialize();

            var newShoppingList = new ShoppingList
                { Id = 1, Name = "Nowa lista", ItemsCount = 0, ItemsBoughtCount = 0 };

            // Act
            _shoppingListServiceMock.Raise(s => s.ShoppingListAdded += null, this, newShoppingList);

            // Assert
            _viewModel.ShoppingLists.Should().HaveCount(1);
            _viewModel.ShoppingLists.First().Name.Should().Be("Nowa lista");
        }

        [Fact]
        public async Task ShoppingListDeleted_Event_ShouldRemoveFromCollection()
        {
            // Arrange
            var shoppingLists = new List<ShoppingList>
            {
                new() { Id = 1, Name = "Lista 1" }
            };

            _userServiceMock.Setup(s => s.ExistsCurrentUser()).ReturnsAsync(true);
            _shoppingListServiceMock.Setup(s => s.GetAll()).ReturnsAsync(shoppingLists);

            await _viewModel.Initialize();

            // Act
            _shoppingListServiceMock.Raise(s => s.ShoppingListDeleted += null, this, 1);

            // Assert
            _viewModel.ShoppingLists.Should().BeEmpty();
        }

        [Fact]
        public async Task ShoppingListUpdated_Event_ShouldUpdateInCollection()
        {
            // Arrange
            var shoppingLists = new List<ShoppingList>
            {
                new() { Id = 1, Name = "Stara nazwa", ItemsCount = 0, ItemsBoughtCount = 0 }
            };

            _userServiceMock.Setup(s => s.ExistsCurrentUser()).ReturnsAsync(true);
            _shoppingListServiceMock.Setup(s => s.GetAll()).ReturnsAsync(shoppingLists);

            await _viewModel.Initialize();

            var updatedShoppingList = new ShoppingList
            {
                Id = 1,
                Name = "Nowa nazwa",
                ItemsCount = 5,
                ItemsBoughtCount = 2
            };

            // Act
            _shoppingListServiceMock.Raise(s => s.ShoppingListUpdated += null, this, updatedShoppingList);

            // Assert
            _viewModel.ShoppingLists.First().Name.Should().Be("Nowa nazwa");
            _viewModel.ShoppingLists.First().ItemsCount.Should().Be(5);
            _viewModel.ShoppingLists.First().ItemsBoughtCount.Should().Be(2);
        }
    }
}