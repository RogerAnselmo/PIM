using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PIM.Api;
using PIM.Api.Data.Context;
using PIM.Api.Infra.AuthEngine.Provider;

namespace PIM.Shared
{
    public class TestInstanceBuilder : TestInstance
    {
        public IConfiguration Configuration;

        public TestInstanceBuilder()
        {
            Configuration = GetConfiguration();
            ConnectionString = Configuration.GetSection("ConnectionStrings:AppConnStr").Value;
            Hash = Configuration.GetSection("token:Hash").Value;
            JwtProvider = new JwtProvider(Configuration);
        }

        public TestInstanceBuilder CreateBackEndServer()
        {
            var builder = new HostBuilder().ConfigureWebHost(webHost =>
            {
                webHost.UseTestServer()
                    .UseStartup<Startup>()
                    .UseConfiguration(Configuration)
                    //.UseEnvironment("Test")
                    .ConfigureLogging(logging =>
                    {
                        logging.SetMinimumLevel(LogLevel.Warning);
                    });
            });

            WebHost = builder.Start();
            HttpClient = WebHost.GetTestServer().CreateClient();
            AutorizedHttpClient = WebHost.GetTestServer().CreateClient();

            return this;
        }

        private static IConfiguration GetConfiguration()
        {
            var backServerPath = Path.Combine(Environment.CurrentDirectory, "..", "..", "..", "..", "..", "src\\PIM.Api");

            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.Test.json", optional: false)
                .SetBasePath(backServerPath)
                .Build();
        }

        public TestInstanceBuilder CreateDataBase()
        {
            var dbOptionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            var sqlOptionsBuilder = new SqlServerDbContextOptionsBuilder(dbOptionsBuilder);

            sqlOptionsBuilder.MigrationsAssembly(typeof(ApplicationContext).GetTypeInfo().Assembly.GetName().Name);
            dbOptionsBuilder.UseSqlServer(ConnectionString);

            ApplicationContext = new ApplicationContext(dbOptionsBuilder.Options);
            ApplicationContext.Database.EnsureDeleted();
            ApplicationContext.Database.Migrate();

            return this;
        }
    }
}
