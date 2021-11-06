using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace PIM.Integration.Test.Spec.Smoke
{
    public class TestingControllerTests : GlobalSetup
    {
        private const string ControllerUrl = "api/Testing";

        class ApplicationHealthTests : TestingControllerTests
        {
            [Test]
            public async Task ApplicationShouldBeAlive()
            {
                Result = await TestInstance.HttpClient.GetAsync($"{ControllerUrl}/ApplicationIsAlive");
                Result.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }
        class AuthorizeTests : TestingControllerTests
        {
            const string AuthUrl = "AuthorizeIsWorking";

            [Test]
            public async Task ShouldReturnOkWhenUserIsAuthorized()
            {
                Result = await TestInstance.AutorizedHttpClient.GetAsync($"{ControllerUrl}/{AuthUrl}");
                Result.StatusCode.Should().Be(HttpStatusCode.OK);
            }

            [Test]
            public async Task ShouldReturn401WhenUserIsNotAuthorized()
            {
                Result = await TestInstance.HttpClient.GetAsync($"{ControllerUrl}/{AuthUrl}");
                Result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            }
        }
    }
}
