using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using PIM.Api.Controllers;
using PIM.Api.Core.Models;
using PIM.Api.Core.Services;
using PIM.Api.TransferObjects.Responses.Base;

namespace PIM.UnitTest.Controllers
{
    public class ProductControllerTests : GlobalSetup
    {
        protected ProductsController ProductsController;
        protected ProductService ProductService;

        [SetUp]
        public void AllClassesSetup()
        {
            ProductService = A.Fake<ProductService>();
            ProductsController = new ProductsController(ProductService);
        }

        public class CreateProduct : ProductControllerTests
        {
            private Product product;
            private BaseResponse resultMessage;
            private const string successMessage = "Product successfully created";

            public class WhenResultIsSuccess : CreateProduct
            {
                [SetUp]
                public async Task SetUp()
                {
                    resultMessage = new BaseResponse(successMessage, true);
                    A.CallTo(() => ProductService.SaveAsync(product)).Returns(Task.FromResult(resultMessage));
                    Result = await ProductsController.CreateProduct(product);
                }

                [Test]
                public void ShouldCallSaveAsync() => A.CallTo(() => ProductService.SaveAsync(product)).MustHaveHappenedOnceExactly();

                [Test]
                public void ShouldReturnCreated() => Result.GetType().Should().Be(typeof(CreatedResult));

                [Test]
                public void ShouldReturnSaveAsyncResult() => Result.Value.Should().BeEquivalentTo(resultMessage);
            }

            public class WhenResultIsError : CreateProduct
            {
                [SetUp]
                public async Task SetUp()
                {
                    resultMessage = new BaseResponse(successMessage, false);
                    A.CallTo(() => ProductService.SaveAsync(product)).Returns(Task.FromResult(resultMessage));
                    Result = await ProductsController.CreateProduct(product);
                }

                [Test]
                public void ShouldCallSaveAsync() => A.CallTo(() => ProductService.SaveAsync(product)).MustHaveHappenedOnceExactly();

                [Test]
                public void ShouldReturnBadRequest() => Result.GetType().Should().Be(typeof(BadRequestObjectResult));

                [Test]
                public void ShouldReturnSaveAsyncResult() => Result.Value.Should().BeEquivalentTo(resultMessage);
            }
        }
    }
}
