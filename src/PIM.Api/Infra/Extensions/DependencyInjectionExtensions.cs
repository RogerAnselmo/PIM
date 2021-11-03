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
            services.AddScoped<SystemUserService>();
            services.AddScoped<AuthService>();

            //Repositories
            services.AddScoped<SystemUserRepository>();
        }
    }
}
