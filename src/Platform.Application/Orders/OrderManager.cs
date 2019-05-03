using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Platform.Authorization.Users;
using Platform.Packages;
using Platform.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Orders
{
    public class OrderManager : DomainService, IOrderManager
    {
        private readonly IRepository<User, long> userRepository;
        private readonly IRepository<Order, long> orderRepository;
        private readonly IRepository<Package, long> packageRepository;

        public OrderManager(IRepository<User, long> userRepository, IRepository<Order, long> orderRepository, IRepository<Package, long> packageRepository)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            this.packageRepository = packageRepository ?? throw new ArgumentNullException(nameof(packageRepository));
        }

        public async Task CancelOrder(long OrderId)
        {
            var order = await orderRepository.FirstOrDefaultAsync(OrderId);
            await orderRepository.DeleteAsync(order);
        }

        public async Task ConfirmOrder(long OrderId)
        {
            var order = await orderRepository.FirstOrDefaultAsync(OrderId);
            if (order.IsCompleted == true)
            {
                throw new ApplicationException("Cannot confirm the order, order is completed already!");
            }
            order.IsActive = true;
        }

        public async Task CompleteOrder(long OrderId)
        {
            var order = await orderRepository.FirstOrDefaultAsync(OrderId);
            if (order.IsActive!=true)
            {
                throw new ApplicationException("Order is not Active, cannot comlpete order!");
            }
            if (order.IsCompleted == true)
            {
                throw new ApplicationException("Order comlpeted already!");
            }
            order.IsCompleted = true;   
        }

        public async Task<Order> CreateOrder(long UserId, ICollection<long> PackageIds)
        {
            var user = await userRepository.GetAllIncluding(u => u.Orders).FirstOrDefaultAsync(u => u.Id == UserId);
            var packages = new List<Package>();
            decimal summ =0;
            foreach(var item in PackageIds)
            {
                var package = await packageRepository.FirstOrDefaultAsync(p => p.Id == item);
                packages.Add(package);
                summ += package.Price;
            }
            var order = new Order { OrderPackages = new List<OrderPackages>(), Summ = summ, User = user };

            var newid = await orderRepository.InsertAndGetIdAsync(order);
            var ord = await orderRepository.FirstOrDefaultAsync(newid);
            foreach(var item in packages)
            {
                ord.OrderPackages.Add(new OrderPackages { Order = ord, Package = item });
            }
            await orderRepository.InsertOrUpdateAsync(ord);
            return await orderRepository.GetAllIncluding(o => o.OrderPackages).FirstOrDefaultAsync(o => o.Id == newid);
        }

        public async Task<Order> GetOrderByIdAsync(long OrderId)
        {
            var order = await orderRepository.GetAllIncluding(o=>o.OrderPackages).FirstOrDefaultAsync(o=>o.Id==OrderId);
            return order;
        }
    }
}
