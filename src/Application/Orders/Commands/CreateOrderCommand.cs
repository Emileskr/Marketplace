using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models.DTOs;
using MediatR;

namespace Application.Orders.Commands;

public record CreateOrderCommand : IRequest<int>
{
    public int UserId { get; set; }
    public int ItemId { get; set; }
};

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
{
    private readonly IJsonPlaceholderClient _client;
    private readonly IOrdersRepository _ordersRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IItemsRepository _itemsRepository;
    public CreateOrderCommandHandler(IJsonPlaceholderClient client, IOrdersRepository ordersRepository, IUsersRepository usersRepository, IItemsRepository itemsRepository)
    {
        _client = client;
        _ordersRepository = ordersRepository;
        _usersRepository = usersRepository;
        _itemsRepository = itemsRepository;
    }

    public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        await ValidateUser(request.UserId);

        await ValidateItem(request.ItemId);

        var orderId = await _ordersRepository.CreateOrder(new NewOrderDto
        {
            ItemId = request.ItemId,
            UserId = request.UserId,
            Status = "created"
        });
        return orderId;
    }
    private async Task ValidateUser(int userId)
    {
        var existingUser = await _usersRepository.GetUser(userId);
        if(existingUser is null)
        {
            var user = await _client.GetUserAsync(userId);
            if (!user.IsSuccessful) throw new UserNotFoundException("User not found");
            await _usersRepository.InsertUser(user.Data!);
        }
    }
    private async Task ValidateItem(int itemId)
    {
        var existingItem = await _itemsRepository.GetItem(itemId);
        if (existingItem is null) throw new ItemNotFoundException("item not found");
    }
}
