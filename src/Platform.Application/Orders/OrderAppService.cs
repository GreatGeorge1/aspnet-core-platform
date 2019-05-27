using Abp.Application.Services;
using Abp.Authorization;
using Platform.Authorization;
using Platform.Orders.Dtos;
using Platform.Packages.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.UI;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Org.BouncyCastle.Ocsp;
using Platform.Professions;
using Platform.Professions.Dtos;
using Platform.Professions.User;

namespace Platform.Orders
{
    [AbpAuthorize]
    public class OrderAppService : ApplicationService
    {
        private readonly OrderManager orderManager;

        public OrderAppService(OrderManager orderManager)
        {
            this.orderManager = orderManager ?? throw new ArgumentNullException(nameof(orderManager));
        }

        public async Task<OrderDto> CreateOrder(CreateOrderDto input)
        {
            var order=await orderManager.CreateOrder(input.UserId, input.PackageId,false);
            return ObjectMapper.Map<OrderDto>(order);
        }

        public async Task CancelOrder(CancelOrderDto input)
        {
            if (input.OrderId == 0)
            {
                throw new UserFriendlyException("id в url не может быть 0 или null");
            }
            var order = await orderManager.GetOrderByIdAsync(input.OrderId);
            if (!PermissionChecker.IsGranted(PermissionNames.Pages_Users))
            {
                if (AbpSession.UserId != order.UserId)
                {
                    throw new AbpAuthorizationException("You are not authorized to cancel this order!");
                }
            }
            await orderManager.CancelOrder(input.OrderId);
        }
        public async Task<ConfirmOrderResponseDto> ConfirmOrder(ConfirmOrderDto input)
        {
            if (input.OrderId == 0)
            {
                throw new UserFriendlyException("OrderId не может быть 0 или null");
            }
            
            if (input.BaseUrl.IsNullOrEmpty())
            {
                throw new UserFriendlyException("BaseUrl не может быть '' или null");
            }
            var order = await orderManager.GetOrderByIdAsync(input.OrderId);
            if (!PermissionChecker.IsGranted(PermissionNames.Pages_Users))
            {
                if (AbpSession.UserId != order.UserId)
                {
                    throw new AbpAuthorizationException("You are not authorized to confirm this order!");
                }
            }
            var response=await orderManager.ConfirmOrder(input.OrderId, input.BaseUrl);
            return response;
        }

        public async Task<ICollection<ProfessionDto>> GetMyCertificateProfessions()
        {
            var userid = AbpSession.UserId ?? 0;
            if (userid != 0)
            {
                var list = await orderManager.GetUserCompletedOrders(userid);
                var result = new List<Profession>();
                foreach (var item in list)
                {
                    result.Add(item.OrderPackages.First().Package.Profession);
                }

                var res = new List<ProfessionDto>();
                foreach (var item in result)
                {
                    res.Add(ObjectMapper.Map<ProfessionDto>(item));
                }
                return res;
            }

            throw new UserFriendlyException("session user not set");
        }
        
        [AllowAnonymous]
        public async Task CompleteOrder(CompleteOrderDto input)
        {
            if (input.OrderId == 0)
            {
                throw new UserFriendlyException("OrderId не может быть 0 или null");
            }
            
            if (input.Secret.IsNullOrEmpty())
            {
                throw new UserFriendlyException("BaseUrl не может быть '' или null");
            }
            await orderManager.CompleteOrder(input.OrderId, input.Secret);
        }
    }

    public class ConfirmOrderDto
    {
        [Required]
        public long OrderId { get; set; }
        [Required]
        public string BaseUrl { get; set; }
    }
    
    public class CancelOrderDto
    {
        [Required]
        public long OrderId { get; set; }
    }
    
    public class CompleteOrderDto
    {
        [Required]
        public long OrderId { get; set; }
        [Required]
        public string Secret { get; set; }
    }
    
    public class ConfirmOrderResponseDto
    {
        [Required]
        public string ForwardUrl { get; set; }
        [Required]
        public double Amount { get; set; }
        //[Required]
        //public string BaseUrl { get; set; }
    }
}
