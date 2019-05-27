using Platform.Packages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Platform.Payment.Models;

namespace Platform.Orders
{
    public interface IOrderManager
    {
        Task<Order> CreateOrder(long UserId, long PackageId, bool isActive,
            Dictionary<string, string> ExtensionData);
        Task CancelOrder(long OrderId);
        Task CompleteOrder(long OrderId);
        Task CompleteOrder(EasyPayNotify notify, string body, string sign);
        Task<Order> GetOrderByIdAsync(long OrderId);
        Task<ICollection<Order>> GetUserCompletedOrders(long userId);
    }
}
