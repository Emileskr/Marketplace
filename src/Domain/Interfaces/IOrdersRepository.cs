
using Domain.Models;
using Domain.Models.DTOs;

namespace Domain.Interfaces;

public interface IOrdersRepository
{
    Task<int> CreateOrder(NewOrderDto order);
    Task<Order> GetOrderById(int id);
    Task UpdateStatus(UpdateOrderStatusModel update);
    Task<IEnumerable<Order>> GetAllOrdersByUser(int userId);
    Task<IEnumerable<Order>> GetCreatedOrders();
    Task UpdateDeletionTime(int orderId, DateTime deleteAt);
    Task<IEnumerable<Order>> GetOrdersMarkedForDeletion();
    Task DeleteOrder(int orderId);
}
