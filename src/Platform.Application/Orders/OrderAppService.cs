using Abp.Application.Services;
using Abp.Authorization;
using Platform.Authorization;
using Platform.Orders.Dtos;
using Platform.Packages.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Orders
{
    [AbpAuthorize]
    public class OrderAppService : ApplicationService
    {
        private readonly OrderManager orderManager;

        public OrderAppService(OrderManager orderManager)
        {
            this.orderManager = orderManager;
        }

        public async Task<OrderDto> CreateOrder(CreateOrderDto input)
        {
            var order=await orderManager.CreateOrder(input.UserId, input.PackageIds);
            return ObjectMapper.Map<OrderDto>(order);
        }

        public async Task CancelOrder(long OrderId)
        {
            if (OrderId == 0)
            {
                throw new ArgumentException("id cannot be 0 or null");
            }
            var order = await orderManager.GetOrderByIdAsync(OrderId);
            if (!PermissionChecker.IsGranted(PermissionNames.Pages_Users))
            {
                if (AbpSession.UserId != order.UserId)
                {
                    throw new AbpAuthorizationException("You are not authorized to cancel this order!");
                }
            }
            await orderManager.CancelOrder(OrderId);
        }
        public async Task ConfirmOrder(long OrderId)
        {
            if (OrderId == 0)
            {
                throw new ArgumentException("id cannot be 0 or null");
            }
            var order = await orderManager.GetOrderByIdAsync(OrderId);
            if (!PermissionChecker.IsGranted(PermissionNames.Pages_Users))
            {
                if (AbpSession.UserId != order.UserId)
                {
                    throw new AbpAuthorizationException("You are not authorized to confirm this order!");
                }
            }
            await orderManager.ConfirmOrder(OrderId);
        }
    }
}
