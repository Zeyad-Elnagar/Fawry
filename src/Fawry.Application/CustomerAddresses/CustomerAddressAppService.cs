using AutoMapper.Internal.Mappers;
using Fawry.Bases;
using Fawry.CustomersAddress;
using Fawry.OrderItems;
using Fawry.Orders.OrderStatus;
using Fawry.Orders;
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

namespace Fawry.CustomerAddresses
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

            var userId = CurrentUser.GetId(); // أو _currentUser.Id.Value;

            var paymentType = await _paymentTypeRepository.FindAsync(input.PaymentTypeId)
                ?? throw new UserFriendlyException("نوع الدفع غير موجود.");

            CustomerAddress? selectedAddress = null;

            if (input.NewAddress != null)
            {
                selectedAddress = ObjectMapper.Map<CreateCustomerAddressDto, CustomerAddress>(input.NewAddress);
                selectedAddress.UserId = userId;
                selectedAddress = await _customerAddressRepository.InsertAsync(selectedAddress, autoSave: true);
                input.CustomerAddressId = selectedAddress.Id;
            }
            else if (input.CustomerAddressId.HasValue)
            {
                selectedAddress = await _customerAddressRepository.GetAsync(input.CustomerAddressId.Value);
                if (selectedAddress.UserId != userId)
                    throw new UserFriendlyException("العنوان لا يخص هذا المستخدم.");
            }
            else
            {
                throw new UserFriendlyException("يجب تحديد عنوان التوصيل.");
            }

            var order = ObjectMapper.Map<CreateOrderDto, Order>(input);
            order.UserId = userId;
            order.OrderDate = DateTime.Now;
            order.Status = OrderStatu.Pending;
            order.TotalAmount = 0;

            await _orderRepository.InsertAsync(order, autoSave: true);

            decimal totalAmount = 0;

            foreach (var item in input.OrderItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                };

                totalAmount += item.Quantity * item.UnitPrice;
                await _orderItemRepository.InsertAsync(orderItem);
            }

            order.TotalAmount = totalAmount;
            await _orderRepository.UpdateAsync(order, autoSave: true);

            return ObjectMapper.Map<Order, OrderDto>(order);
        }


        public async Task<OrderDto> ConfirmOrderPaymentAsync(int orderId)
        {
            var order = await _orderRepository.GetAsync(orderId);
            if (order == null)
                throw new EntityNotFoundException(typeof(Order), orderId);

            // ✅ تحقق من طريقة الدفع
            var paymentType = await _paymentTypeRepository.FindAsync(order.PaymentTypeId);
            if (paymentType == null)
                throw new UserFriendlyException("نوع الدفع المحدد غير موجود.");

            var orderItems = await _orderItemRepository.GetListAsync(x => x.OrderId == orderId);

            foreach (var item in orderItems)
            {
                var product = await _productRepository.GetAsync(item.ProductId);

                if (product.StockQuantity < item.Quantity)
                    throw new UserFriendlyException($"المنتج {product.Name} لا يحتوي على الكمية المطلوبة.");

                product.StockQuantity -= item.Quantity;
                await _productRepository.UpdateAsync(product);
            }

            order.Status = OrderStatu.Delivered; // أو أي حالة مناسبة
            await _orderRepository.UpdateAsync(order);

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
            var order = await _orderRepository.GetAsync(id);
            var orderDto = ObjectMapper.Map<Order, OrderDto>(order);

            var paymentType = await _paymentTypeRepository.GetAsync(order.PaymentTypeId);
            orderDto.PaymentTypeName = paymentType?.Name;

            var orderItems = await _orderItemRepository.GetListAsync(x => x.OrderId == id);
            orderDto.OrderItems = ObjectMapper.Map<List<OrderItem>, List<OrderItemDto>>(orderItems);

            return orderDto;
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
