using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PIM.Api.Core.Models;
using PIM.Api.Data.Context;
using PIM.Api.Data.Repositories.Base;
using PIM.Api.TransferObjects.Requests;

namespace PIM.Api.Data.Repositories
{
    public class ProductRepository : BaseRepository<Product>
    {
        public ProductRepository(ApplicationContext db) : base(db)
        {
        }

        public virtual async Task<Product> GetByName(string name) =>
            await Db.Products.Where(x => x.Name.ToUpper()
                    .Equals(name.ToUpper()))
                .FirstOrDefaultAsync();


        public virtual IEnumerable<Product> GetByFilter(ProductsFilterModel filter) =>
            Db.Products.Where(x => (filter.Color.IsNullOrEmpty() || x.Color.ToUpper().Contains(filter.Color.ToUpper()))
                                   && (filter.Category.IsNullOrEmpty() || x.Category.ToUpper().Contains(filter.Category.ToUpper()))
                                   && (filter.Name.IsNullOrEmpty() || x.Name.ToUpper().Contains(filter.Name.ToUpper()))
                                   && (filter.Brand.IsNullOrEmpty() || x.Brand.ToUpper().Contains(filter.Brand.ToUpper()))
                                   )
                .Skip(filter.Skip)
                .Take(filter.GetPageSize());

        public virtual IEnumerable<Product> GetByIdRange(List<int> idList)
        {   
            return Db.Products.Where(x => idList.Contains(x.Id));
        }
    }
}
