using AutoMapper.Internal.Mappers;
using Fawry.Bases;
using Fawry.CustomerAddresses;
using Fawry.CustomersAddress;
using Fawry.OrderItems;
using Fawry.Orders.OrderStatus;
using Fawry.PaymentTypes;
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

namespace Fawry.Orders
{
    [RemoteService(false)]
    public class OrderAppService : BaseApplicationService, IOrderAppService
    {

        private readonly IOrderRepository _orderRepository;
        private readonly IRepository<OrderItem, int> _orderItemRepository;
        private readonly IRepository<PaymentType, int> _paymentTypeRepository;
        private readonly IRepository<Product, int> _productRepository;
        private readonly IRepository<CustomerAddress, int> _customerAddressRepository;


        public OrderAppService
                (IOrderRepository orderRepository
                , IRepository<OrderItem, int> orderItemRepository
                , IRepository<PaymentType, int> paymentTypeRepository
                , IRepository<Product, int> productRepository
                , IRepository<CustomerAddress, int> customerAddressRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _paymentTypeRepository = paymentTypeRepository;
            _productRepository = productRepository;
            _customerAddressRepository = customerAddressRepository;
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto input)
        {
            if (input.OrderItems == null || !input.OrderItems.Any())
                throw new UserFriendlyException("يجب تحديد منتجات للطلب.");

            var userId = CurrentUser.GetId();

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Status = OrderStatu.Pending,
                TotalAmount = 0,
                OrderItems = new List<OrderItem>()
            };

            decimal totalAmount = 0;

            foreach (var item in input.OrderItems)
            {
                var product = await _productRepository.GetAsync(item.ProductId); // احصل على السعر من المنتج

                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price, // هنا السعر من قاعدة البيانات
                    TotalPrice = product.Price * item.Quantity
                };

                totalAmount += orderItem.TotalPrice;
                order.OrderItems.Add(orderItem);
            }

            order.TotalAmount = totalAmount;

            await _orderRepository.InsertAsync(order, autoSave: true);

            return ObjectMapper.Map<Order, OrderDto>(order);
        }


        public async Task<OrderDto> ConfirmOrderAsync(int orderId, ConfirmOrderDto input)
        {
            var userId = CurrentUser.GetId();

            var order = await _orderRepository.FirstOrDefaultAsync(x => x.Id == orderId && x.UserId == userId);
            if (order == null)
                throw new UserFriendlyException("الطلب غير موجود.");

            if (order.OrderItems == null || !order.OrderItems.Any())
                throw new UserFriendlyException("الطلب لا يحتوي على منتجات.");

            var address = await _customerAddressRepository.FirstOrDefaultAsync(x => x.Id == input.CustomerAddressId && x.UserId == userId);
            if (address == null)
                throw new UserFriendlyException("عنوان التوصيل غير صالح.");

            var paymentType = await _paymentTypeRepository.FindAsync(input.PaymentTypeId);
            if (paymentType == null)
                throw new UserFriendlyException("نوع الدفع غير موجود.");

            order.CustomerAddressId = input.CustomerAddressId;
            order.PaymentTypeId = input.PaymentTypeId;

            decimal totalAmount = 0;
            var orderItems = await _orderItemRepository.GetListAsync(x => x.OrderId == order.Id);

            foreach (var item in orderItems)
            {
                var product = await _productRepository.GetAsync(item.ProductId);

                if (product.StockQuantity < item.Quantity)
                    throw new UserFriendlyException($"المنتج '{product.Name}' لا يحتوي على الكمية المطلوبة.");

                product.StockQuantity -= item.Quantity;
                await _productRepository.UpdateAsync(product);

                totalAmount += item.Quantity * item.UnitPrice;
            }

            order.TotalAmount = totalAmount;
            order.Status = OrderStatu.Delivered;

            await _orderRepository.UpdateAsync(order, autoSave: true);

            return ObjectMapper.Map<Order, OrderDto>(order);
        }



        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetAsync(id);
            if (order == null)
            {
                throw new EntityNotFoundException(typeof(Order), id);
            }
            await _orderRepository.DeleteAsync(order, autoSave: true);
            return true;
        }

        public async Task<PagedResultDto<OrderDto>> GetListAsync(GetOrderListDto input)
        {
            var userId = CurrentUser.GetId();

            var query = await _orderRepository.GetQueryableAsync();

            query = query.Where(x => x.UserId == userId);

            if (input.Status.HasValue)
                query = query.Where(x => x.Status == input.Status.Value);

            var totalCount = await AsyncExecuter.CountAsync(query);
            var orders = await AsyncExecuter.ToListAsync(
                query.Skip(input.SkipCount).Take(input.MaxResultCount)
            );

            var orderDtos = ObjectMapper.Map<List<Order>, List<OrderDto>>(orders);

            return new PagedResultDto<OrderDto>(
                totalCount,
                orderDtos
            );
        }


        public async Task<OrderDto> GetOrderAsync(int id)
        {
            var userId = CurrentUser.GetId();
            var order = await _orderRepository.GetAsync(id, includeDetails: true);

            if (order.UserId != userId)
                throw new UserFriendlyException("لا يمكنك عرض هذا الطلب.");

            return new OrderDto
            {
                Id = order.Id,
                TotalAmount = order.TotalAmount,
                OrderItems = ObjectMapper.Map<List<OrderItem>, List<OrderItemDto>>(order.OrderItems.ToList())
            };
        }



        public async Task<OrderDto> UpdateOrderAsync(int id, UpdateOrderDto input)
        {
            var order = await _orderRepository.FindAsync(id);
            if (order == null)
            {
                throw new EntityNotFoundException(typeof(Order), id);
            }
            // تحديث الطلب
            ObjectMapper.Map(input, order);
            await _orderRepository.UpdateAsync(order, autoSave: true);

            return ObjectMapper.Map<Order, OrderDto>(order);
        }
    }
}
