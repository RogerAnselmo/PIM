using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PIM.Api.Core.Models;
using PIM.Api.Core.Services;
using PIM.Api.Data.Repositories;
using PIM.Api.TransferObjects.Requests;
using PIM.Shared.Builders;

namespace PIM.Integration.Test.Spec.Services
{
    class ProductServiceTests : GlobalSetup
    {
        private ProductService _productService;
        private ProductRepository _productRepository;
        private ProductBuilder _productBuilder;
        private ProductRequestModel _product;
        private Product _productInDataBase;

        [OneTimeSetUp]
        public void AllClassesOneTimeSetUp()
        {
            _productBuilder = new ProductBuilder(TestInstance.ApplicationContext);
            _productRepository = new ProductRepository(TestInstance.ApplicationContext);
            _productService = new ProductService(_productRepository);
        }

        class SaveAsync : ProductServiceTests
        {
            class WhenThereIsNoProductWithSameName : SaveAsync
            {
                [OneTimeSetUp]
                public async Task OneTimeSetUp()
                {
                    _product = _productBuilder.CreateRequestModel();
                    await _productService.SaveAsync(_product);
                    _productInDataBase = await TestInstance.ApplicationContext.Products.FirstOrDefaultAsync();
                }

                [Test]
                public void ShouldSaveInDataBase()
                {
                    var expectedValues = new Product().SetValues(_product);
                    expectedValues.Id = _productInDataBase.Id;
                    _productInDataBase.Should().BeEquivalentTo(expectedValues);
                }
            }

            class WhenThereIsSomeProductWithSameName : SaveAsync
            {
                private ProductRequestModel _productWithSameName;

                [OneTimeSetUp]
                public async Task OneTimeSetUp()
                {
                    _productInDataBase = await _productBuilder.CreateInDataBase();
                    _productWithSameName = _productBuilder
                        .WithName(_productInDataBase.Name).
                        CreateRequestModel();
                    
                    await _productService.SaveAsync(_productWithSameName);
                }

                [Test]
                public async Task ShouldNotSaveInDataBase()
                {
                    var product = await TestInstance.ApplicationContext.Products.FindAsync(_productWithSameName.Id);
                    product.Should().BeNull();
                }
            }
        }

        class UpdateAsync : ProductServiceTests
        {
            private ProductRequestModel _updatedProduct;

            [OneTimeSetUp]
            public void UpdateOneTimeSetup() =>
                _updatedProduct = new ProductRequestModel
                {
                    Name = Faker.Random.Words(),
                    Category = Faker.Random.Words(),
                    Color = Faker.Random.Words(),
                    Brand = Faker.Random.Words(),
                    Description = Faker.Random.Words(),
                    Price = Faker.Random.Decimal(),
                };

            class WhenProductIsValid : UpdateAsync
            {
                [OneTimeSetUp]
                public async Task OneTimeSetUp()
                {
                    _productInDataBase = await _productBuilder.CreateInDataBase();
                    _updatedProduct.Id = _productInDataBase.Id;

                    await _productService.UpdateAsync(_updatedProduct);
                }

                [Test]
                public async Task ShouldSaveInDataBase()
                {
                    var productInDatabase = await TestInstance.ApplicationContext.Products.FindAsync(_updatedProduct.Id);
                    var productWithNewValues = _productInDataBase.SetValues(_updatedProduct);

                    productInDatabase.Should().BeEquivalentTo(productWithNewValues);
                }
            }

            class WhenProductIsNotFound : UpdateAsync
            {
                [OneTimeSetUp]
                public async Task OneTimeSetUp()
                {
                    _productInDataBase = await _productBuilder.CreateInDataBase();
                    _updatedProduct.Id = _productInDataBase.Id + Faker.Random.Int(1);

                    await _productService.UpdateAsync(_updatedProduct);
                }
                    
                [Test]
                public async Task ShouldNotUpdateDataBase()
                {
                    var productInDatabase = await TestInstance.ApplicationContext.Products.FindAsync(_productInDataBase.Id);
                    var productWithNewValues = _productInDataBase.SetValues(_updatedProduct);
                    productInDatabase.Should().BeEquivalentTo(productWithNewValues);
                }
            }
        }
    }
}
