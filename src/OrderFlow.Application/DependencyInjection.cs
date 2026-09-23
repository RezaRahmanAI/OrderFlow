using Microsoft.Extensions.DependencyInjection;
using OrderFlow.Application.Products.CreateProduct;
using OrderFlow.Application.Products.UpdateProduct;

namespace OrderFlow.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateProductService>();
        services.AddScoped<UpdateProductService>();

        return services;
    }
}
