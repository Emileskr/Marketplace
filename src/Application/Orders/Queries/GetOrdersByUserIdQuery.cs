using Domain.Interfaces;
using Domain.Models;
using MediatR;


namespace Application.Orders.Queries;

public class GetOrdersByUserIdQuery : IRequest<List<Order>>
{
    public int UserId { get; set; }
}
public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, List<Order>>
{
    private readonly IOrdersRepository _ordersRepository;
    public GetOrdersByUserIdQueryHandler(IOrdersRepository ordersRepository)
    {
        _ordersRepository = ordersRepository;
    }

    public async Task<List<Order>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
    {
        var orders = await _ordersRepository.GetAllOrdersByUser(request.UserId);
        return orders.ToList();
    }
}

