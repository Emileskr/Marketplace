using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data;

namespace Marketplace.IntegrationTests;

public class CustomWebApplicationFactory<TProgram>
       : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var sp = services.BuildServiceProvider();

            services.AddTransient<IDbConnection>(d => new NpgsqlConnection(sp.GetRequiredService<IConfiguration>().GetConnectionString("TestDatabaseConnection")));

        });

        builder.UseEnvironment("Test");
    }
}
