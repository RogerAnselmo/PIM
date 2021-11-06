using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PIM.Api.Core.Models;
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

            [Test]
            public void ShouldCreateOnlyOneProduct()
            {
                TestInstance
                    .ApplicationContext
                    .Products
                    .Count()
                    .Should()
                    .Be(1);
            }
        }
    }
}
