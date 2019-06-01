using System.Threading.Tasks;
using Abp.Domain.Services;
using Platform.Payment.Dtos;
using Platform.Payment.Models;

namespace Platform.Payment
{
    public interface IPaymentService:IDomainService
    {
        Task<bool> CheckSign(string body, string sign);
        Task<CreateApp> CreateApp();
        Task<CreateOrderResponse> CreateOrder(CreateApp createApp, string orderid, string description, double amount);
        Task<CreateOrderResponse> CreateOrder(CreateApp createApp, string orderid, string description, double amount, Urls urls);
    }
}