using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection service)
    {
        service.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        service.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
