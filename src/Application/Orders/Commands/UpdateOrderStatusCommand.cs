using Application.Validators;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using Domain.Models.DTOs;
using MediatR;

namespace Application.Orders.Commands
{
    public record UpdateOrderStatusCommand : IRequest
    {
        public int OrderId { get; set; }
        public string Status { get; set; } = "created";
    };
    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand>
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly UpdateOrderStatusCommandValidator _validator;
        public UpdateOrderStatusCommandHandler(IOrdersRepository ordersRepository, UpdateOrderStatusCommandValidator validator)
        {
            _ordersRepository = ordersRepository;
            _validator = validator;
        }
        public async Task Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            ValidateCommand(request);
            var existingOrder = await _ordersRepository.GetOrderById(request.OrderId) ?? throw new OrderNotFoundException("Order was not found.");

            await _ordersRepository.UpdateStatus(new UpdateOrderStatusModel
            {
                OrderId = existingOrder.Id,
                Status = request.Status,
            });
        }
        private void ValidateCommand(UpdateOrderStatusCommand command)
        {
            var result = _validator.Validate(command);
            if (!result.IsValid) throw new InvalidUpdateStatusCommandException("Invalid status.");
        }
    }
}
