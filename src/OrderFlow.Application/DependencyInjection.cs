using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using OrderFlow.Application.Authentication.Register;
using OrderFlow.Application.Authentication.Login;
using OrderFlow.Application.Authentication.Refresh;
using OrderFlow.Application.Authentication.Logout;
using OrderFlow.Application.Products.DeactivateProduct;
using OrderFlow.Application.Products.GetProducts;
using OrderFlow.Application.Products.GetProductById;
using OrderFlow.Application.Products.CreateProduct;
using OrderFlow.Application.Products.UpdateProduct;

namespace OrderFlow.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();
        services.AddScoped<CreateProductService>();
        services.AddScoped<UpdateProductService>();
        services.AddScoped<DeactivateProductService>();
        services.AddScoped<GetProductsService>();
        services.AddScoped<GetProductByIdService>();
        services.AddScoped<RegisterService>();
        services.AddScoped<LoginService>();
        services.AddScoped<RefreshService>();
        services.AddScoped<LogoutService>();

        return services;
    }
}
