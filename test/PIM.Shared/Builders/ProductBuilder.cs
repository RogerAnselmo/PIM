using System.Threading.Tasks;
using PIM.Api.Core.Models;
using PIM.Api.Data.Context;
using PIM.Api.TransferObjects.Requests;
using PIM.Shared.Builders.Base;

namespace PIM.Shared.Builders
{
    public class ProductBuilder : BaseBuilder<Product>
    {
        public ProductBuilder(ApplicationContext builderContext) : base(builderContext)
        {
        }

        public override void ResetModel() =>
            Model = new Product
            {
                Brand = Faker.Random.Word(),
                Category = Faker.Random.Word(),
                Color = Faker.Random.Word(),
                Description = Faker.Random.Words(),
                Name = Faker.Random.Words(),
                Price = Faker.Random.Decimal(1M, 500M),
            };

        public override Product CreateInMemory() =>
            new()
            {
                Brand = Model.Brand ?? Faker.Random.Word(),
                Category = Model.Category ?? Faker.Random.Word(),
                Color = Model.Color ?? Faker.Random.Word(),
                Description = Model.Description ?? Faker.Random.Words(),
                Name = Model.Name ?? Faker.Random.Words(),
                Price = Model.Price,
            };

        public ProductBuilder WithName(string name)
        {
            Model.Name = name;
            return this;
        }

        public ProductRequestModel CreateRequestModel() =>
            new ProductRequestModel
            {
                Name = Model.Name,
                Category = Model.Category,
                Color = Model.Color,
                Brand = Model.Brand,
                Description = Model.Description,
                Id = Model.Id,
                Price = Model.Price
            };

        public override async Task<Product> CreateInDataBase()
        {
            var obj = CreateInMemory();
            await BuilderContext.Products.AddAsync(obj);
            await BuilderContext.SaveChangesAsync();
            ResetModel();
            return obj;
        }
    }
}
