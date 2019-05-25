using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Services;
using Abp.Logging;
using Abp.UI;
using Castle.Core.Logging;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Platform.Payment.Dtos;
using Platform.Payment.Models;

namespace Platform.Payment
{
    public class EasyPayService:DomainService,IPaymentService
    {
        private ILogger _logger { get; set; }
        private const string testhost = "https://api.easypay.ua";
        private const string secret = "test";
        private const string partnerKey = "easypay-test";
        private const string serviceKey = "MERCHANT-TEST";

        public EasyPayService()
        {
            _logger =  Logger = NullLogger.Instance;;
        }

        public async Task<CreateApp> CreateApp()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                    var content = new MultipartContent();
                    content.Headers.Add("PartnerKey", partnerKey);
                    content.Headers.Add("locale", "ua");
                    var res = await client.PostAsync($"{testhost}/api/system/createApp", content);
                    //_logger.Info(res.Content.ReadAsStringAsync().Result);
                    var createApp = JsonConvert.DeserializeObject<CreateApp>(res.Content.ReadAsStringAsync().Result);
                    _logger.Info($"Easypay appId:{createApp.AppId}, pageId:{createApp.PageId}, apiVersion:{createApp.ApiVersion}");
                    return createApp;
                }
                catch (Exception e)
                {
                    throw new UserFriendlyException(e.Message);
                }
                
            }
        }
        
        public async Task<CreateOrderResponse> CreateOrder(CreateApp createApp, long orderid, string description, double amount)
        {
            using (HttpClient client = new HttpClient())
            {
               // try
                {
                    using (var request = new HttpRequestMessage(HttpMethod.Post,$"{testhost}/api/merchant/createOrder" ))
                    {
                        var createDto = new Payment.Dtos.CreateOrderMinDto()
                        {
                            Order = new OrderMin()
                            {
                                ServiceKey = serviceKey,
                                OrderId = orderid.ToString(),
                                Description = description,
                                Amount=amount
                            }
                        };
                        var json = JsonConvert.SerializeObject(createDto);
                        using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                        {
                            stringContent.Headers.Add("PartnerKey", partnerKey);
                            stringContent.Headers.Add("locale", "ua");
                            stringContent.Headers.Add("AppId", createApp.AppId);
                            stringContent.Headers.Add("PageId",createApp.PageId);
                            var sign = Sign(json, secret);
                            stringContent.Headers.Add("Sign", sign);
                            
                            request.Content = stringContent;
                            using (var response = await client
                                .SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                            {
                              //  response.EnsureSuccessStatusCode();
                                var createOrder = JsonConvert.DeserializeObject<CreateOrderResponse>(response.Content.ReadAsStringAsync().Result);
                                return createOrder;
                            }
                        }
                    }
                }
               // catch (Exception e)
               // {
                   // throw new UserFriendlyException(e.Message);
               // }
                
            }
        }

        private string Sign(string requestBody,string secret)
        {
            return Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(secret+requestBody)));
        }
        
        private string CreateToken(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA1(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }
        public static string ComputeHash(string plainText, string salt)
        {
            // Convert plain text into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

            SHA256Managed hash = new SHA256Managed();

            // Compute hash value of salt.
            byte[] plainHash = hash.ComputeHash(plainTextBytes);

            byte[] concat = new byte[plainHash.Length + saltBytes.Length];

            System.Buffer.BlockCopy(saltBytes, 0, concat, 0, saltBytes.Length);
            System.Buffer.BlockCopy(plainHash, 0, concat, saltBytes.Length, plainHash.Length);

            byte[] tHashBytes = hash.ComputeHash(concat);

            // Convert result into a base64-encoded string.
            string hashValue = Convert.ToBase64String(tHashBytes);

            // Return the result.
            return hashValue;
        }
    }
}