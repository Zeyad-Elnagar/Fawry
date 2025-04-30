using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fawry.Orders.OrderStatus
{
    public enum OrderStatu
    {
        Pending = 0,      // تم إنشاء الطلب
        InPreparation = 1, // يتم تحضيره
        InTransit = 2,    // خرج للتوصيل
        Delivered = 3     // تم التسليم
    }
}
