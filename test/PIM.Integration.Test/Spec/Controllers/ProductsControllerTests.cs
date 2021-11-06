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
            private UpdateProductRequestModel _updateProductRequestModel;

            [OneTimeSetUp]
            public async Task OneTimeSetUp()
            {
                _product = await _productBuilder.CreateInDataBase();
                
                _updateProductRequestModel = new UpdateProductRequestModel
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
    }
}
