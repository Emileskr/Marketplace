using Application.Orders.Commands;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Marketplace.IntegrationTests;

public class OrdersControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    public OrdersControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
    }
    [Theory]
    [AutoData]
    public async Task GetByUserId_ShouldReturnSuccess(int userId)
    {
        var result = await _client.GetAsync($"/ordersFromUser?userId={userId}");

        result.EnsureSuccessStatusCode();
    }
    [Fact]
    public async Task CreateOrder_ShouldReturnCreatedStatus()
    {
        // ARRANGE
        var newOrder = new CreateOrderCommand
        {
            UserId = 1,
            ItemId = 2
        };

        // ACT
        var response = await _client.PostAsJsonAsync("/orders", newOrder);

        // ASSERT
        response.EnsureSuccessStatusCode();
    }
}
