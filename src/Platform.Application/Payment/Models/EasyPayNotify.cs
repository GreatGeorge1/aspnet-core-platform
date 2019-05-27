using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Platform.Payment.Models
{
    public class EasyPayNotify
    {
        [JsonProperty("action")] 
        public NotifyActionType Action { get; set; }
        [JsonProperty("merchant_id")] 
        public string MerchantId { get; set; }
        [JsonProperty("order_id")] 
        public string OrderId { get; set; }
        [JsonProperty("version")] 
        public string Version { get; set; }
        [JsonProperty("date")] 
        public DateTime Date { get; set; }
        [JsonProperty("details")] 
        public NotifyDetails Details { get; set; }
    }

    public class NotifyDetails
    {
        [JsonProperty("amount")] 
        public double Amount { get; set; }
        [JsonProperty("desc")] 
        public string Description { get; set; }
        [JsonProperty("payment_id")] 
        public long PaymentId { get; set; }
        [JsonProperty("recurrent_id")] 
        public dynamic? RecurrentId { get; set; }//????
    }

    [JsonConverter(typeof(StringEnumConverter), true)]
    public enum NotifyActionType
    {
        Payment
    }
}