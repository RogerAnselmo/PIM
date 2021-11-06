using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PIM.Api.Core.Models;
using PIM.Api.Data.Repositories;
using PIM.Api.TransferObjects.Requests;
using PIM.Api.TransferObjects.Responses.Base;

namespace PIM.Api.Core.Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly ProductRepository _productRepository;

        public OrderService(OrderRepository orderRepository, ProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<BaseResponse> SaveAsync(NewOrderRequest newOrderRequest)
        {
            var order = new Order(newOrderRequest);
            var productList = _productRepository.GetByIdRange(newOrderRequest.ProductIdList);
            
            order.SetProductList(productList);

            if(!order.Products.Any())
                return new BaseResponse("No products were found", false);

            order.Price = productList?.Sum(x => x.Price);

            await _orderRepository.SaveAndCommitAsync(order);
            return new BaseResponse("Order successfully saved", true);
        }

        public IEnumerable<Order> GetAll()
        {
            return _orderRepository.GetOrders();
        }
    }
}
