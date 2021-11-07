using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using PIM.Api.Core.Models;
using PIM.Api.Core.Services;
using PIM.Api.Data.Repositories;
using PIM.Api.TransferObjects.Requests;
using PIM.Api.TransferObjects.Responses.Base;

namespace PIM.UnitTest.Core.Services
{
    public class ProductServiceTests : GlobalSetup
    {
        private ProductRepository _productRepository;
        private ProductService _productService;
        private ProductRequestModel _product;

        [OneTimeSetUp]
        public void AllClassesOneTimeSetup()
        {
            _product = new ProductRequestModel
            {
                Id = Faker.Random.Int(),
                Brand = Faker.Random.Words(),
                Category = Faker.Random.Words(),
                Color = Faker.Random.Words(),
                Description = Faker.Random.Words(),
                Name = Faker.Random.Words(),
                Price = Faker.Random.Decimal(),
            };

            _productRepository = A.Fake<ProductRepository>();
            _productService = new ProductService(_productRepository);
        }

        class SaveAsync : ProductServiceTests
        {
            private BaseResponse _response;
            private Product newProduct;

            class WhenThereIsNoProductWithSameName : SaveAsync
            {
                [OneTimeSetUp]
                public async Task OneTimeSetup()
                {
                    _response = new BaseResponse("Product successfully created", true);

                    A.CallTo(() => _productRepository.GetByName(_product.Name))
                        .Returns(Task.FromResult(null as Product));
                    newProduct = new Product().SetValues(_product);
                    ResponseResult = await _productService.SaveAsync(_product);
                }

                [Test]
                public void ShouldCallGetByName() =>
                    A.CallTo(() => _productRepository.GetByName(_product.Name))
                        .MustHaveHappened();

                [Test]
                public void ShouldCallSaveAsync()
                {
                    A.CallTo(() => _productRepository.SaveAndCommitAsync(A<Product>.That.Matches(x =>
                            x.Id.Equals(newProduct.Id) &&
                            x.Name.Equals(newProduct.Name) &&
                            x.Brand.Equals(newProduct.Brand) &&
                            x.Category.Equals(newProduct.Category) &&
                            x.Color.Equals(newProduct.Color) &&
                            x.Description.Equals(newProduct.Description) &&
                            x.Price.Equals(newProduct.Price)
                        )))
                        .MustHaveHappened();
                }

                [Test]
                public void ShouldReturnResponseObject() => ResponseResult.Should().BeEquivalentTo(_response);
            }

            class WhenThereIsSomeProductWithSameName : SaveAsync
            {
                [OneTimeSetUp]
                public async Task OneTimeSetup()
                {
                    _response = new BaseResponse($"Name {_product.Name} is already in use", false);

                    A.CallTo(() => _productRepository.GetByName(_product.Name))
                        .Returns(Task.FromResult(new Product()));

                    newProduct = new Product().SetValues(_product);
                    ResponseResult = await _productService.SaveAsync(_product);
                }

                [Test]
                public void ShouldCallGetByName() =>
                    A.CallTo(() => _productRepository.GetByName(_product.Name))
                        .MustHaveHappened();

                [Test]
                public void ShouldNotCallSaveAsync() =>
                    A.CallTo(() => _productRepository.SaveAndCommitAsync(A<Product>._))
                        .MustNotHaveHappened();

                [Test]
                public void ShouldReturnResponseObject() => ResponseResult.Should().BeEquivalentTo(_response);
            }
        }

        class GetByFilter : ProductServiceTests
        {
            private ProductsFilterModel _filter;

            class WhenFilterIsNull : GetByFilter
            {
                [OneTimeSetUp]
                public void OneTimeSetup()
                {
                    _filter = new ProductsFilterModel();
                    _productService.GetByFilter(null);
                }

                [Test]
                public void ShouldCallGetByFilterWithNewFilter() =>
                    A.CallTo(() => _productRepository.GetByFilter(
                        A<ProductsFilterModel>.That.Matches(x =>
                            x.Name.Equals(_filter.Name) &&
                            x.Category.Equals(_filter.Category) &&
                            x.Color.Equals(_filter.Color) &&
                            x.Brand.Equals(_filter.Brand) &&
                            x.Description.Equals(_filter.Description) &&
                            x.Page.Equals(_filter.Page) &&
                            x.PageSize.Equals(_filter.PageSize)
                        )))
                        .MustHaveHappenedOnceExactly();
            }

            class WhenFilterIsNotNull : GetByFilter
            {
                [OneTimeSetUp]
                public void OneTimeSetup()
                {
                    _filter = new ProductsFilterModel
                    {
                        Name = Faker.Random.Words(),
                        Brand = Faker.Random.Words(),
                        Category = Faker.Random.Words(),
                        Color = Faker.Random.Words(),
                        Description = Faker.Random.Words(),
                        PageSize = Faker.Random.Int(),
                        Page = Faker.Random.Int()
                    };

                    _productService.GetByFilter(_filter);
                }


                [Test]
                public void ShouldCallGetByFilterWithCorrectFilter() =>
                    A.CallTo(() => _productRepository.GetByFilter(_filter))
                        .MustHaveHappenedOnceExactly();
            }
        }

        class UpdateAsync : ProductServiceTests
        {
            private ProductRequestModel updatedProduct;

            [OneTimeSetUp]
            public void UpdateOneTimeSetup() =>
                updatedProduct = new ProductRequestModel
                {
                    Name = Faker.Random.Words(),
                    Category = Faker.Random.Words(),
                    Color = Faker.Random.Words(),
                    Brand = Faker.Random.Words(),
                    Description = Faker.Random.Words(),
                    Id = Faker.Random.Int(),
                    Price = Faker.Random.Decimal(),
                };

            class WhenProductIsNotFound : UpdateAsync
            {
                [OneTimeSetUp]
                public async Task OneTimeSetup()
                {
                    A.CallTo(() => _productRepository.GetAsync(updatedProduct.Id))
                        .Returns(Task.FromResult(null as Product));

                    ResponseResult = await _productService.UpdateAsync(updatedProduct);
                }

                [Test]
                public void ShouldCallGetAsync() => A.CallTo(() => _productRepository.GetAsync(updatedProduct.Id)).MustHaveHappenedOnceExactly();

                [Test]
                public void ShouldReturnResponseObject() => ResponseResult.Should().BeEquivalentTo(new BaseResponse("Product not found", false));
            }

            class WhenProductIsValid : UpdateAsync
            {
                private Product product;

                [OneTimeSetUp]
                public async Task OneTimeSetup()
                {
                    product = new Product();

                    A.CallTo(() => _productRepository.GetAsync(updatedProduct.Id))
                        .Returns(Task.FromResult(product));

                    ResponseResult = await _productService.UpdateAsync(updatedProduct);
                }

                [Test]
                public void ShouldCallGetAsync() => A.CallTo(() => _productRepository.GetAsync(updatedProduct.Id)).MustHaveHappenedOnceExactly();

                [Test]
                public void ShouldCallUpdateAndCommitAsync() => A.CallTo(() => _productRepository.UpdateAndCommitAsync(product)).MustHaveHappenedOnceExactly();

                [Test]
                public void ShouldReturnResponseObject() => ResponseResult.Should().BeEquivalentTo(new BaseResponse("Product successfully updated ", true));
            }
        }
    }
}
