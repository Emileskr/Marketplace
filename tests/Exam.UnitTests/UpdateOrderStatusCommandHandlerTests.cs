using Application.Orders.Commands;
using Application.Validators;
using AutoFixture.Xunit2;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using FluentAssertions;
using Moq;

namespace Marketplace.UnitTests
{
    public class UpdateOrderStatusCommandHandlerTests
    {
        private readonly UpdateOrderStatusCommandValidator _validator;
        private readonly Mock<IOrdersRepository> _mockRepository;
        private readonly UpdateOrderStatusCommandHandler _handler;
        public UpdateOrderStatusCommandHandlerTests()
        {
            _mockRepository = new Mock<IOrdersRepository>();
            _validator = new UpdateOrderStatusCommandValidator();
            _handler = new UpdateOrderStatusCommandHandler(_mockRepository.Object, _validator);
        }
        [Theory]
        [AutoData]
        public async Task InvalidStatus_ThrowsInvalidUpdateStatusCommandException(Order order)
        {
            // ARRANGE
            string status = "done";

            _mockRepository.Setup(x => x.GetOrderById(It.IsAny<int>())).ReturnsAsync(order);

            // ACT
            Func<Task> act = async () => await _handler.Handle(new UpdateOrderStatusCommand { OrderId =  order.Id, Status = status }, CancellationToken.None);

            // ASSERT
            await act.Should().ThrowAsync<InvalidUpdateStatusCommandException>().WithMessage("Invalid status.");
        }
        [Theory]
        [AutoData]
        public async Task ValidStatus_ShouldCallUpdateStatus_InRepository (Order order)
        {
            // ARRANGE
            string status = "completed";

            _mockRepository.Setup(x => x.GetOrderById(It.IsAny<int>())).ReturnsAsync(order);

            // ACT
            await _handler.Handle(new UpdateOrderStatusCommand { OrderId = order.Id, Status = status }, CancellationToken.None);

            // ASSERT
            _mockRepository.Verify(i => i.UpdateStatus(It.IsAny<UpdateOrderStatusModel>()), Times.Once());
        }
    }
}
