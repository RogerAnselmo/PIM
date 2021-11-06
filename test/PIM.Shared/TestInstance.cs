using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PIM.Api.Data.Context;
using PIM.Api.Infra.AuthEngine.Provider;
using Respawn;

namespace PIM.Shared
{
    public class TestInstance
    {
        protected TestInstance()
        {
            Checkpoint = new Checkpoint
            {
                TablesToIgnore = new[] { "__EFMigrationsHistory" }
            };
        }
        public IHost WebHost;
        public HttpClient HttpClient;
        public HttpClient AutorizedHttpClient;
        public Checkpoint Checkpoint;
        public ApplicationContext ApplicationContext;
        public JwtProvider JwtProvider;
        public string Hash;
        public string ConnectionString { get; protected set; }

        public async Task ResetDatabase()
        {
            if (ApplicationContext != null)
            {
                await ApplicationContext.Database.OpenConnectionAsync();
                await Checkpoint.Reset(ApplicationContext.Database.GetDbConnection());
                await ApplicationContext.Database.CloseConnectionAsync();
            }
        }
    }
}
