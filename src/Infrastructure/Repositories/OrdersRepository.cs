using Dapper;
using Domain.Interfaces;
using Domain.Models;
using Domain.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly IDbConnection _connection;
    public OrdersRepository(IDbConnection connection)
    {
        _connection = connection;
    }
    public async Task<int> CreateOrder(NewOrderDto order)
    {
        string sql = @"INSERT INTO orders (item_id, user_id, status) VALUES
                    (@ItemId, @UserId, @Status) RETURNING id;";
        var args = new
        {
            ItemId = order.ItemId,
            UserId = order.UserId,
            Status = order.Status
        };
        return await _connection.QuerySingleAsync<int>(sql, args);
    }


    public async Task<Order> GetOrderById(int id)
    {
        string sql = @"SELECT id, item_id, user_id, created_at, status 
                        FROM orders
                        WHERE id = @Id;";
        return await _connection.QueryFirstOrDefaultAsync<Order>(sql, new { Id = id });
    }

    public async Task UpdateStatus(UpdateOrderStatusModel update)
    {
        string sql = @"UPDATE orders 
                        SET status = @Status
                        WHERE Id = @OrderId;";
        await _connection.ExecuteAsync(sql, new {Status =  update.Status, OrderId = update.OrderId});

    }
    public async Task<IEnumerable<Order>> GetAllOrdersByUser(int userId)
    {
        string sql = @"SELECT id, item_id, user_id, created_at, status 
                        FROM orders
                        WHERE user_id = @Id;";
        return await _connection.QueryAsync<Order>(sql, new { Id = userId });
    }
    public async Task<IEnumerable<Order>> GetCreatedOrders()
    {
        return await _connection.QueryAsync<Order>(
                @"SELECT id, item_id, user_id, created_at, status, delete_at 
                FROM orders 
                WHERE status = 'created' AND delete_at IS NULL");
    }
    public async Task UpdateDeletionTime(int orderId, DateTime deleteAt)
    {
        await _connection.ExecuteAsync(
                    @"UPDATE orders SET delete_at = @DeleteAt WHERE id = @OrderId",
                    new { DeleteAt = deleteAt, OrderId = orderId }
                );
    }
    public async Task<IEnumerable<Order>> GetOrdersMarkedForDeletion()
    {
        return await _connection.QueryAsync<Order>(
                @"SELECT id, item_id, user_id, created_at, status, delete_at 
                    FROM orders 
                    WHERE status = 'created' 
                    AND delete_at IS NOT NULL");
    }
    public async Task DeleteOrder(int orderId)
    {
        await _connection.ExecuteAsync("DELETE FROM orders WHERE id = @OrderId", new { OrderId = orderId });
    }

}
