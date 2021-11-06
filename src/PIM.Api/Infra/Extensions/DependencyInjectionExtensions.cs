using Microsoft.Extensions.DependencyInjection;
using PIM.Api.Core.Services;
using PIM.Api.Data.Repositories;
using PIM.Api.Infra.AuthEngine.Interface;
using PIM.Api.Infra.AuthEngine.Provider;

namespace PIM.Api.Infra.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            //Services
            services.AddSingleton<ITokenProvider, JwtProvider>();
            services.AddScoped<AuthService>();
            services.AddScoped<SystemUserService>();
            services.AddScoped<ProductService>();
            services.AddScoped<OrderService>();

            //Repositories
            services.AddScoped<SystemUserRepository>();
            services.AddScoped<ProductRepository>();
            services.AddScoped<OrderRepository>();
        }
    }
}
