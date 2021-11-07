using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using PIM.Api.Core.Models;
using PIM.Api.TransferObjects.Requests;
using PIM.Shared.Builders;

namespace PIM.Integration.Test.Spec.Controllers
{
    class ProductsControllerTests : GlobalSetup
    {
        private const string ControllerUrl = "api/Products";
        private ProductBuilder _productBuilder;
        private Product _product;

        [OneTimeSetUp]
        public void AllClassesOneSetUp() => _productBuilder = new ProductBuilder(TestInstance.ApplicationContext);

        class CreateProduct : ProductsControllerTests
        {
            private const string MethodUrl = "CreateProduct";

            [OneTimeSetUp]
            public async Task OneTimeSetUp()
            {
                _product = _productBuilder.CreateInMemory();
                Result = await TestInstance.AutorizedHttpClient.PostAsync($"{ControllerUrl}/{MethodUrl}", ToHttpContent(_product));
            }

            [Test]
            public void ShouldReturn201() => Result.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        class UpdateProduct : ProductsControllerTests
        {
            private const string MethodUrl = "UpdateProduct";
            private ProductRequestModel _updateProductRequestModel;

            [OneTimeSetUp]
            public async Task OneTimeSetUp()
            {
                _product = await _productBuilder.CreateInDataBase();
                
                _updateProductRequestModel = new ProductRequestModel
                {
                    Id = _product.Id,
                    Name = Faker.Random.Word(),
                    Category = Faker.Random.Word(),
                    Brand = Faker.Random.Word(),
                    Color = Faker.Random.Word(),
                    Description = Faker.Random.Words(),
                    Price = Faker.Random.Decimal(10, 5000)
                };

                Result = await TestInstance.AutorizedHttpClient.PutAsync($"{ControllerUrl}/{MethodUrl}",
                    ToHttpContent(_updateProductRequestModel));
            }
            
            [Test]
            public void ShouldReturn200() => Result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        class GetByFilter : ProductsControllerTests
        {
            private ProductsFilterModel filter;
            private const string MethodUrl = "GetByFilter";

            [OneTimeSetUp]
            public async Task OneTimeSetUp()
            {
                filter = new ProductsFilterModel
                {
                    Name = Faker.Random.Words(),
                    Brand = Faker.Random.Words(),
                    Category = Faker.Random.Words(),
                    Color = Faker.Random.Words(),
                    Description = Faker.Random.Words(),
                    PageSize = Faker.Random.Int(),
                    Page = Faker.Random.Int()
                };

                var parameters = $"?Page={filter.Page}&PageSize={filter.PageSize}&Name={filter.Name}&Description={filter.Description}&Brand={filter.Brand}&Category={filter.Category}&Color={filter.Color}";
                Result = await TestInstance.AutorizedHttpClient.GetAsync($"{ControllerUrl}/{MethodUrl}{parameters}");
            }

            [Test]
            public void ShouldReturn200() => Result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
