using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Newtonsoft.Json;
using NUnit.Framework;
using PIM.Api.Core.Models;
using PIM.Shared;

namespace PIM.Integration.Test
{
    public class GlobalSetup
    {
        protected HttpResponseMessage Result;
        public static Faker Faker = new Faker("pt_BR");
        public static TestInstance TestInstance;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            TestInstance = new TestInstanceBuilder()
                .CreateDataBase()
                .CreateBackEndServer();

            ConfigAuthorize();
        }

        [TearDown]
        public async Task AllTestsTearDown() =>
            await TestInstance.ResetDatabase();

        [OneTimeTearDown]
        public void OneTimeTearDown() => TestInstance.WebHost?.Dispose();

        protected HttpContent ToHttpContent(object loginUser)
        {
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(loginUser), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return httpContent;
        }

        private void ConfigAuthorize()
        {
            var token = TestInstance.JwtProvider.GenerateToken(TestInstance.Hash, new SystemUser{UserName = "testingUser" });
            TestInstance.AutorizedHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
