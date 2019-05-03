using Platform.Packages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Orders
{
    public interface IOrderManager
    {
        Task<Order> CreateOrder(long UserId, ICollection<long> PackageIds);
        Task CancelOrder(long OrderId);
        Task CompleteOrder(long OrderId);
        Task<Order> GetOrderByIdAsync(long OrderId);
    }
}
