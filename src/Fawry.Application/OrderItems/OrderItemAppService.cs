using AutoMapper.Internal.Mappers;
using Fawry.Bases;
using Fawry.Orders.OrderStatus;
using Fawry.Orders;
using Fawry.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using Volo.Abp;

namespace Fawry.OrderItems
{
    [RemoteService(false)]
    public class OrderItemAppService : BaseApplicationService, IOrderitemAppService
    {
        private readonly IRepository<OrderItem, int> _orderItemRepository;
        private readonly IRepository<Product, int> _productRepository;
        private readonly IRepository<Order, int> _orderRepository;

        public OrderItemAppService(
            IRepository<OrderItem, int> orderItemRepository,
            IRepository<Product, int> productRepository,
            IRepository<Order, int> orderRepository)
        {
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public async Task<OrderItemDto> CreateOrderItemAsync(CreateOrderItemDto input)
        {
            var currentUserId = CurrentUser.GetId();

            var order = await _orderRepository.FirstOrDefaultAsync(o => o.UserId == currentUserId && o.Status == OrderStatu.Pending);

            if (order == null)
            {
                order = new Order
                {
                    UserId = currentUserId,
                    Status = OrderStatu.Pending,
                    OrderDate = DateTime.Now
                };
                await _orderRepository.InsertAsync(order, autoSave: true);
            }

            var product = await _productRepository.GetAsync(input.ProductId)
                ?? throw new UserFriendlyException("المنتج غير موجود.");

            if (input.Quantity <= 0)
                throw new UserFriendlyException("الكمية غير صالحة.");

            if (product.StockQuantity < input.Quantity)
                throw new UserFriendlyException("الكمية المطلوبة غير متوفرة حالياً.");

            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                ProductId = input.ProductId,
                Quantity = input.Quantity,
                UnitPrice = product.Price
            };

            await _orderItemRepository.InsertAsync(orderItem, autoSave: true);

            return ObjectMapper.Map<OrderItem, OrderItemDto>(orderItem);
        }


        public async Task<bool> DeleteOrderItemAsync(int id)
        {
            var orderitem = await _orderItemRepository.GetAsync(id);
            if (orderitem == null)
            {
                throw new EntityNotFoundException(typeof(OrderItem), id);
            }
            await _orderItemRepository.DeleteAsync(orderitem, autoSave: true);
            return true;
        }

        public async Task<PagedResultDto<OrderItemDto>> GetOrderItemsListAsync(int orderId)
        {
            var items = await _orderItemRepository.GetListAsync(x => x.OrderId == orderId);
            var count = items.Count;

            return new PagedResultDto<OrderItemDto>(
                count,
                ObjectMapper.Map<List<OrderItem>, List<OrderItemDto>>(items)
            );
        }

        public async Task<OrderItemDto> UpdateOrderItemAsync(int id, UpdateOrderItemDto input)
        {
            var orderItem = await _orderItemRepository.FindAsync(id);
            if (orderItem == null)
            {
                throw new EntityNotFoundException(typeof(OrderItem), id);
            }
            orderItem.Quantity = input.Quantity;
            orderItem.UnitPrice = input.UnitPrice;

            await _orderItemRepository.UpdateAsync(orderItem, autoSave: true);

            return ObjectMapper.Map<OrderItem, OrderItemDto>(orderItem);
        }
    }
}
