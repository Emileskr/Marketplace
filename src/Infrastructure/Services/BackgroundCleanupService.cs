using Application.Orders.Commands;
using Dapper;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services;

internal class BackgroundCleanupService : BackgroundService
{
    private readonly ILogger<BackgroundCleanupService> _logger;
    private readonly IConfiguration _configuration;
    private IServiceProvider Services { get; }
    private readonly IOrdersRepository _ordersRepository;
    public BackgroundCleanupService(ILogger<BackgroundCleanupService> logger, IConfiguration configuration, IServiceProvider services, IOrdersRepository ordersRepository)
    {
        _logger = logger;
        _configuration = configuration;
        Services = services;
        _ordersRepository = ordersRepository;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);

                await CheckAndCleanupOrders();

                _logger.LogInformation("Timed Hosted Service Cleaned up.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in the background service. Cleanup not accomplished.");
        }

    }
    private async Task CheckAndCleanupOrders()
    {
        using (var scope = Services.CreateScope())
        {
            var createdOrders = await _ordersRepository.GetCreatedOrders();

            foreach (var order in createdOrders)
            {
                DateTime deletionTime = order.CreatedAt.AddHours(2);
                await _ordersRepository.UpdateDeletionTime(order.Id, deletionTime);
            }

            var ordersMarkedForDeletion = await _ordersRepository.GetOrdersMarkedForDeletion();

            foreach (var order in ordersMarkedForDeletion)
            {
                if (DateTime.UtcNow.AddHours(2) >= order.DeleteAt)
                {
                    await _ordersRepository.DeleteOrder(order.Id);
                }
            }
        }
    }
}
