using System.Collections.Generic;
using System.Threading.Tasks;
using PIM.Api.Core.Models;
using PIM.Api.Data.Repositories;
using PIM.Api.TransferObjects.Requests;
using PIM.Api.TransferObjects.Responses.Base;

namespace PIM.Api.Core.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;

        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public virtual async Task<BaseResponse> SaveAsync(Product product)
        {
            var productWithTheSameName = await _productRepository.GetByName(product.Name);
            if (productWithTheSameName != null)
                return new BaseResponse($"Name {product.Name} is already in use", false);

            await _productRepository.SaveAndCommitAsync(product);
            return new BaseResponse("Product successfully created", true);
        }

        public virtual IEnumerable<Product> GetByFilter(ProductsFilterModel filter)
        {
            return _productRepository.GetByFilter(filter);
        }

        public virtual async Task<BaseResponse> UpdateAsync(UpdateProductRequestModel updatedProduct)
        {
            var product = await _productRepository.GetAsync(updatedProduct.Id);

            if(product == null)
                return new BaseResponse("Product not found", false);

            product.UpdateValues(updatedProduct);
            await _productRepository.UpdateAndCommitAsync(product);
            return new BaseResponse("Product successfully updated ", true);
        }
    }
}
