using FluentAssertions;
using NUnit.Framework;
using PIM.Api.Core.Models;
using PIM.Api.TransferObjects.Requests;

namespace PIM.UnitTest.Core.Models
{
    public class ProductTests: GlobalSetup
    {
        private Product _product;

        class UpdateValues: ProductTests
        {
            private ProductRequestModel _requestModel;
            private int _originalId;

            [SetUp]
            public void Setup()
            {
                _originalId = Faker.Random.Int();
                _product = new Product {Id = _originalId};

                _requestModel = new ProductRequestModel
                {
                    Name = Faker.Random.Words(),
                    Category = Faker.Random.Words(),
                    Brand = Faker.Random.Words(),
                    Color = Faker.Random.Words(),
                    Description = Faker.Random.Words(),
                    Price = Faker.Random.Decimal(10, 5000),
                    Id = Faker.Random.Int()
                };

                _product.SetValues(_requestModel);
            }

            [Test]
            public void ShouldUpdateName() => _product.Name.Should().Be(_requestModel.Name);

            [Test]
            public void ShouldUpdateBrand() => _product.Brand.Should().Be(_requestModel.Brand);

            [Test]
            public void ShouldUpdateCategory() => _product.Category.Should().Be(_requestModel.Category);

            [Test]
            public void ShouldUpdateColor() => _product.Color.Should().Be(_requestModel.Color);

            [Test]
            public void ShouldUpdateDescription() => _product.Description.Should().Be(_requestModel.Description);

            [Test]
            public void ShouldUpdatePrice() => _product.Price.Should().Be(_requestModel.Price);

            [Test]
            public void ShouldNotUpdateId() => _product.Id.Should().Be(_originalId);
        }
    }
}
