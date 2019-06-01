using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Platform.Authorization.Users;
using Platform.Packages;
using Platform.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Domain.Entities;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.UI;
using Microsoft.AspNetCore.WebUtilities;
using Nito.AsyncEx;
using Platform.Background;
using Platform.Payment;
using Platform.Payment.Dtos;
using Platform.Payment.Models;
using Order = Platform.Packages.Order;

namespace Platform.Orders
{
    public class OrderManager : DomainService, IOrderManager
    {
        private readonly IRepository<User, long> userRepository;
        private readonly IRepository<Order, long> orderRepository;
        private readonly IRepository<Package, long> packageRepository;
        private readonly EasyPayService _paymentService;
        private readonly IBackgroundJobManager _backgroundJobManager; 

        public OrderManager(IRepository<User, long> userRepository, 
            IRepository<Order, long> orderRepository, 
            IRepository<Package, long> packageRepository,
            EasyPayService paymentService,
            IBackgroundJobManager backgroundJobManager)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            this.packageRepository = packageRepository ?? throw new ArgumentNullException(nameof(packageRepository));
            _paymentService=paymentService?? throw new ArgumentNullException(nameof(paymentService));
            _backgroundJobManager=backgroundJobManager?? throw new ArgumentNullException(nameof(backgroundJobManager));
        }

        public async Task CancelOrder(long OrderId)
        {
            var order = await orderRepository.FirstOrDefaultAsync(OrderId);
            await orderRepository.DeleteAsync(order);
        }

        [UnitOfWork]
        public async Task<ConfirmOrderResponseDto> ConfirmOrder(long OrderId, string baseUrl)
        {
            var order = await orderRepository.GetAll()
                .Include(o=>o.User)
                .Include(o=>o.OrderPackages)
                .ThenInclude(o=>o.Package)
                .ThenInclude(o=>o.Profession)
                .ThenInclude(o=>o.Content)
                .FirstOrDefaultAsync(o=>o.Id==OrderId);
            if (order.IsCompleted == true)
            {
                throw new UserFriendlyException("Cannot confirm the order, order is completed already!");
            }
            order.IsActive = true;
            var user = order.User;
            var app=await _paymentService.CreateApp();

            var secret = UrlToken.GenerateToken();
            //var secret2 = UrlToken.GenerateToken();
            order.SetData("secretSuccess",secret);
           // order.SetData("secretFailed",secret);
            try
            {
                var createOrder = await _paymentService.CreateOrder(app, order.Id.ToString(), $"Оплата сертификата #{order.Id}.Користувач {user.Name}, Id #{user.Id}, {user.EmailAddress}. Курс {order.OrderPackages.First().Package.Profession.Content.Title} Id#{order.OrderPackages.First().Package.Profession.Id}",
                    (double) order.Summ, new Urls()
                    {
                        Success = $"{baseUrl}/?orderId={OrderId}&success=true&secret={secret}",
                        Failed = $"{baseUrl}/?orderId={OrderId}&success=false&secret=null"
                    });
                return new ConfirmOrderResponseDto()
                {
                    ForwardUrl = createOrder.ForwardUrl,
                    Amount = createOrder.Amount
                };
            }
            catch (HttpRequestException e)
            {
                await orderRepository.DeleteAsync(OrderId);
                throw new UserFriendlyException(e.Message);
            }
        }

        public async Task CompleteOrder(long OrderId, string secret)
        {
            var order = await orderRepository.FirstOrDefaultAsync(OrderId);
            if (order.IsActive!=true)
            {
                throw new UserFriendlyException("Order is not Active, cannot complete order!");
            }
            if (order.IsCompleted == true)
            {
                throw new UserFriendlyException("Order completed already!");
            }

            var token = order.GetData<string>("secretSuccess");
            if (token.Equals(secret))
            {
                order.IsCompleted = true;   
            }
            else
            {
                throw new UserFriendlyException("Secret not valid");
            }
        }
        
        public async Task CompleteOrder(long OrderId)
        {
            var order = await orderRepository.FirstOrDefaultAsync(OrderId);
            if (order.IsActive!=true)
            {
                throw new UserFriendlyException("Order is not Active, cannot complete order!");
            }
            if (order.IsCompleted == true)
            {
                //throw new UserFriendlyException("Order completed already!");
            }
            else
            {
                order.IsCompleted = true;
            }
        }
        [UnitOfWork]
        public async Task CompleteOrder(EasyPayNotify notify, string body, string sign)
        {
            var check=await _paymentService.CheckSign(body, sign);
            if (check == false)
            {
                _ = await _backgroundJobManager.EnqueueAsync<SendEMailJob, SendEmailArgs>(
                    new SendEmailArgs
                    {
                        Email = "info@choizy.org",
                        Subject = $"Отладка бек(ошибка easypay)",
                        isHtml = true,
                        Message = $@"{body}<br><br>
                                    {sign}
                             "
                    });
                //throw new UserFriendlyException("Sign not valid");
                return;
            }
            var order = await orderRepository.FirstOrDefaultAsync(Convert.ToInt64(notify.OrderId));
            if (order.IsActive!=true)
            {
                //throw new UserFriendlyException("Order is not Active, cannot complete order!");
                return;
            }

            if (order.IsCompleted == true)
            {

            }
            else
            {
                order.IsCompleted = true;
            }

            var fullorder = await orderRepository.GetAll()
                .Include(o => o.User)
                .Include(o=>o.OrderPackages)
                    .ThenInclude(o=>o.Package).ThenInclude(o=>o.Profession).ThenInclude(o=>o.Content)
                .FirstOrDefaultAsync(o => o.Id == Convert.ToInt64(notify.OrderId));
            var user = order.User;
            var package = order.OrderPackages.First().Package;
            var profession = package.Profession;
            _ = await _backgroundJobManager.EnqueueAsync<SendEMailJob, SendEmailArgs>(
                new SendEmailArgs
                {
                    Email = "info@choizy.org",
                    Subject = $"Оплата успішна - {user.EmailAddress}",
                    isHtml = true,
                    Message = $@"Ім'я: <b>{user.Name}</b><br><br>
                                Email: <a href = 'mailto: {user.EmailAddress}'>{user.EmailAddress}</a><br><br>
                                Телефон: <b>{user.PhoneNumber}</b><br><br>
                                Курс <b>{profession.Content.Title}</b><br><br>
                                Сума <b>{notify.Details.Amount}</b><br>
                                Опис <b>{notify.Details.Description}</b><br>
                             "
                });
        }

        [UnitOfWork]
        public async Task<Order> CreateOrder(long UserId, long PackageId,bool isActive, Dictionary<string,string> ExtensionData=null)
        {
            var user = await userRepository.GetAllIncluding(u => u.Orders).FirstOrDefaultAsync(u => u.Id == UserId);
            var packages = new List<Package>();

            var package = await packageRepository.FirstOrDefaultAsync(p => p.Id == PackageId);
            decimal summ =0;
            summ += package.Price;
            var orderlist=await orderRepository.GetAll()
                .Include(o=>o.OrderPackages)
                    .ThenInclude(o=>o.Package)
                .Include(o=>o.User)
                .Where(o=>o.User.Id==user.Id)
                .ToListAsync();
            if (orderlist.Any())
            {
                foreach (var item in orderlist)
                {
                    foreach (var orderpackage in item.OrderPackages)
                    {
                        if (orderpackage.PackageId == PackageId)
                        {
                            item.Summ = summ;
                            return item;
                        }
                    }
                }
            }
            
            packages.Add(package);
            //summ += package.Price;
            
            var order = new Order { OrderPackages = new List<OrderPackages>(), Summ = summ, User = user };
            order.IsActive = isActive;
            if (ExtensionData != null)
            {
                foreach (var item in ExtensionData)
                {
                    order.SetData(item.Key, item.Value);
                }
            }
            
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
        
        public async Task<ICollection<Order>> GetUserCompletedOrders(long userId)
        {
            var list = await orderRepository.GetAll()
                .Include(o => o.OrderPackages)
                .ThenInclude(o => o.Package)
                .ThenInclude(p => p.Profession)
                .ThenInclude(p => p.Content)
                .Where(o=>o.IsCompleted)
                .Where(o => o.User.Id == userId)
                .ToListAsync();
            return list;
        } 
    }
    
    public class UrlToken
    {
        private const int BYTE_LENGTH = 32; 

        /// <summary>
        /// Generate a fixed length token that can be used in url without endcoding it
        /// </summary>
        /// <returns></returns>
        public static string GenerateToken()
        {
            // get secure array bytes
            byte[] secureArray = GenerateRandomBytes();

            // convert in an url safe string
            string urlToken = WebEncoders.Base64UrlEncode(secureArray);

            return urlToken;
        }

        /// <summary>
        /// Generate a cryptographically secure array of bytes with a fixed length
        /// </summary>
        /// <returns></returns>
        private static byte[] GenerateRandomBytes()
        {
            using (RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider()) { 
                byte[] byteArray = new byte[BYTE_LENGTH];
                provider.GetBytes(byteArray);

                return byteArray;
            }
        }
    }
}
