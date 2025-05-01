using Fawry.CustomerAddresses;
using Fawry.OrderItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;
using Fawry.Orders.OrderStatus;
using Fawry.PaymentTypes;
namespace Fawry.Orders
{
    public class Order : FullAuditedEntity<int>
    {
        public Guid UserId { get; set; }
        public virtual IdentityUser User { get; set; }
        public int? CustomerAddressId { get; set; }
        public virtual CustomerAddress CustomerAddress { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatu Status { get; set; } = OrderStatu.Pending;
        public decimal TotalAmount { get; set; }
        public int? PaymentTypeId { get; set; }
        public virtual PaymentType PaymentType { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
