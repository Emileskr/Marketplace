using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.Orders.Queries;

public class GetOrderByIdQuery : IRequest<Order>
{
    public int Id { get; set; }
}
public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Order>
{
    private readonly IOrdersRepository _ordersRepository;
    public GetOrderByIdQueryHandler(IOrdersRepository ordersRepository)
    {
        _ordersRepository = ordersRepository;
    }

    public async Task<Order> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
       return await _ordersRepository.GetOrderById(request.Id);
    }
}
