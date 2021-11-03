using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PIM.Api.Infra.Extensions;

namespace PIM.Api
{
    public class Startup
    {
        private const string CorsPolicyName = "ALLOW_FRONTEND";
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSwagger();
            services.AddDependencyInjection();
            services.AddContext(Configuration.GetValue<string>("ConnectionStrings:AppConnStr"));
            //services.AddCors(CorsPolicyName, Configuration.GetValue<string>("FrontEnd:BaseUrl"));
            services.AddTokenAuthorization(Configuration.GetValue<string>("token:Hash"));
            services.AddControllers();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            //app.AddCors(CorsPolicyName);
            app.RunMigrations();
            app.AddTokenAuthorization();
            app.AddSwagger(); app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
