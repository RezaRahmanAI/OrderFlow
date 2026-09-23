using Microsoft.Extensions.DependencyInjection;
using OrderFlow.Application.Products.CreateProduct;

namespace OrderFlow.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateProductService>();

        return services;
    }
}
