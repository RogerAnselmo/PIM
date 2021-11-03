using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PIM.Api.Data.Context;

namespace PIM.Api.Infra.Extensions
{
    public static class ContextExtensions
    {
        public static void AddContext(this IServiceCollection services, string connectionString) =>
            services.AddDbContext<ApplicationContext>(x =>
                x.UseSqlServer(connectionString));

        public static void RunMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var ctx = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            ctx.Database.Migrate();
        }
    }
}
