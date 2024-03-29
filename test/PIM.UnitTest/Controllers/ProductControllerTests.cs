﻿using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using PIM.Api.Controllers;
using PIM.Api.Core.Services;
using PIM.Api.TransferObjects.Requests;
using PIM.Api.TransferObjects.Responses.Base;

namespace PIM.UnitTest.Controllers
{
    public class ProductControllerTests : GlobalSetup
    {
        protected ProductsController ProductsController;
        protected ProductService ProductService;
        protected BaseResponse ResultObject;
        protected const string ResultMessage = "any message";

        [SetUp]
        public void AllClassesSetup()
        {
            ProductService = A.Fake<ProductService>();
            ProductsController = new ProductsController(ProductService);
        }

        public class CreateProduct : ProductControllerTests
        {
            private ProductRequestModel _product;

            public class WhenResultIsSuccess : CreateProduct
            {
                [SetUp]
                public async Task SetUp()
                {
                    _product = new ProductRequestModel
                    {
                        Brand = Faker.Random.Words(),
                        Category = Faker.Random.Words(),
                        Color = Faker.Random.Words(),
                        Description = Faker.Random.Words(),
                        Name = Faker.Random.Words(),
                    };

                    ResultObject = new BaseResponse(ResultMessage, true);
                    A.CallTo(() => ProductService.SaveAsync(_product)).Returns(Task.FromResult(ResultObject));
                    HttpResultResult = await ProductsController.CreateProduct(_product);
                }

                [Test]
                public void ShouldCallSaveAsync() => A.CallTo(() => ProductService.SaveAsync(_product)).MustHaveHappenedOnceExactly();

                [Test]
                public void ShouldReturnCreated() => HttpResultResult.GetType().Should().Be(typeof(CreatedResult));

                [Test]
                public void ShouldReturnSaveAsyncResult() => HttpResultResult.Value.Should().BeEquivalentTo(ResultObject);
            }

            public class WhenResultIsError : CreateProduct
            {
                [SetUp]
                public async Task SetUp()
                {
                    ResultObject = new BaseResponse(ResultMessage, false);
                    A.CallTo(() => ProductService.SaveAsync(_product)).Returns(Task.FromResult(ResultObject));
                    HttpResultResult = await ProductsController.CreateProduct(_product);
                }

                [Test]
                public void ShouldCallSaveAsync() => A.CallTo(() => ProductService.SaveAsync(_product)).MustHaveHappenedOnceExactly();

                [Test]
                public void ShouldReturnBadRequest() => HttpResultResult.GetType().Should().Be(typeof(BadRequestObjectResult));

                [Test]
                public void ShouldReturnSaveAsyncResult() => HttpResultResult.Value.Should().BeEquivalentTo(ResultObject);
            }
        }

        public class UpdateProduct : ProductControllerTests
        {
            private ProductRequestModel _requestModel;
            public class WhenResultIsSuccess : UpdateProduct
            {
                [SetUp]
                public async Task SetUp()
                {
                    _requestModel = new ProductRequestModel
                    {
                        Name = Faker.Random.Words(),
                        Category = Faker.Random.Words(),
                        Brand = Faker.Random.Words(),
                        Color = Faker.Random.Words(),
                        Description = Faker.Random.Words()
                    };

                    ResultObject = new BaseResponse(ResultMessage, true);
                    A.CallTo(() => ProductService.UpdateAsync(_requestModel)).Returns(Task.FromResult(ResultObject));
                    HttpResultResult = await ProductsController.UpdateProduct(_requestModel);
                }

                [Test]
                public void ShouldCallSaveAsync() => A.CallTo(() => ProductService.UpdateAsync(_requestModel)).MustHaveHappenedOnceExactly();

                [Test]
                public void ShouldReturnOk() => HttpResultResult.GetType().Should().Be(typeof(OkObjectResult));

                [Test]
                public void ShouldReturnSaveAsyncResult() => HttpResultResult.Value.Should().BeEquivalentTo(ResultObject);
            }

            public class WhenResultIsError : UpdateProduct
            {
                [SetUp]
                public async Task SetUp()
                {
                    ResultObject = new BaseResponse(ResultMessage, false);
                    A.CallTo(() => ProductService.UpdateAsync(_requestModel)).Returns(Task.FromResult(ResultObject));
                    HttpResultResult = await ProductsController.UpdateProduct(_requestModel);
                }

                [Test]
                public void ShouldCallSaveAsync() => A.CallTo(() => ProductService.UpdateAsync(_requestModel)).MustHaveHappenedOnceExactly();

                [Test]
                public void ShouldReturnBadRequest() => HttpResultResult.GetType().Should().Be(typeof(BadRequestObjectResult));

                [Test]
                public void ShouldReturnUpdateAsyncResult() => HttpResultResult.Value.Should().BeEquivalentTo(ResultObject);
            }
        }
    }
}
