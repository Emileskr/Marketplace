using Application.Orders.Commands;
using AutoFixture.Xunit2;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using Domain.Models.DTOs;
using FluentAssertions;
using Moq;

namespace Marketplace.UnitTests;

public class CreateOrderCommandHandlerTests
{
    private readonly Mock<IJsonPlaceholderClient> _client;
    private readonly Mock<IOrdersRepository> _ordersRepository;
    private readonly Mock<IUsersRepository> _usersRepository;
    private readonly Mock<IItemsRepository> _itemsRepository;
    private readonly CreateOrderCommandHandler _handler;
    public CreateOrderCommandHandlerTests()
    {
        _client =  new Mock<IJsonPlaceholderClient>();
        _ordersRepository = new Mock<IOrdersRepository>();
        _usersRepository = new Mock<IUsersRepository>();
        _itemsRepository = new Mock<IItemsRepository>();
        _handler = new CreateOrderCommandHandler(_client.Object, _ordersRepository.Object, _usersRepository.Object, _itemsRepository.Object);
    }
    [Theory]
    [AutoData]
    public async Task CreateOrder_UserNotSavedToDatabase_CallsCorrectDependencies(CreateOrderCommand command, Item item)
    {
        // ARRANGE
        _usersRepository.Setup(x => x.GetUser(It.IsAny<int>())).ReturnsAsync((UserDto?)null);
        _itemsRepository.Setup(x => x.GetItem(It.IsAny<int>())).ReturnsAsync(item);
        _client.Setup(x => x.GetUserAsync(It.IsAny<int>())).ReturnsAsync(new JsonPlaceholderResult<UserDto> { IsSuccessful = true });

        // ACT
        await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        _client.Verify(i => i.GetUserAsync(command.UserId), Times.Once);
        _ordersRepository.Verify(i => i.CreateOrder(It.IsAny<NewOrderDto>()), Times.Once);
    }
    [Theory]
    [AutoData]
    public async Task CreateOrder_ItemNotExisting_ThrowsItemNotFoundException(CreateOrderCommand command, UserDto user)
    {
        // ARRANGE
        _usersRepository.Setup(x => x.GetUser(It.IsAny<int>())).ReturnsAsync(user);
        _itemsRepository.Setup(x => x.GetItem(It.IsAny<int>())).ReturnsAsync((Item?)null);

        // ACT
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        await act.Should().ThrowAsync<ItemNotFoundException>().WithMessage("item not found");
    }
}
