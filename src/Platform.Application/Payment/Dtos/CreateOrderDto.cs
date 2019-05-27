using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Platform.Packages;
using Platform.Payment.Models;

namespace Platform.Payment.Dtos
{
    public class CreateOrderDto
    {
        [JsonProperty("userInfo")]
        public UserInfo UserInfo { get; set; }
        [JsonProperty("order")]
        public Payment.Dtos.Order Order { get; set; }
        [JsonProperty("urls")]
        public Urls Urls { get; set; }
        [JsonProperty("bankingDetailsId")]
        public string BankingDetailsId { get; set; }
        [JsonProperty("bankingDetails")]
        public BankingDetails BankingDetails { get; set; }
        [JsonProperty("reccurent")]
        public Recurrent Recurrent { get; set; } 
        [JsonProperty("splitting")]
        public Splitting Splitting { get; set; }
        [JsonProperty("userPaymentInstrument")]
        public UserPaymentInstrument UserPaymentInstrument { get; set; }
        [JsonProperty("useLoyality")]
        public bool? useLoyality { get; set; }
    }
    
    public class CreateOrderMinDto
    {
       // [JsonProperty("userInfo")]
        //public UserInfo UserInfo { get; set; }
        [JsonProperty("order")]
        public Payment.Dtos.OrderMin Order { get; set; }
        [JsonProperty("urls")]
        public Urls Urls { get; set; }
        //[JsonProperty("urls")]
        //public Urls Urls { get; set; }
//        [JsonProperty("bankingDetailsId")]
//        public string BankingDetailsId { get; set; }
//        [JsonProperty("bankingDetails")]
//        public BankingDetails BankingDetails { get; set; }
//        [JsonProperty("reccurent")]
//        public Recurrent Recurrent { get; set; } 
//        [JsonProperty("splitting")]
//        public Splitting Splitting { get; set; }
//        [JsonProperty("userPaymentInstrument")]
//        public UserPaymentInstrument UserPaymentInstrument { get; set; }
//        [JsonProperty("useLoyality")]
//        public bool? useLoyality { get; set; }
    }
    
    public class OrderMin
    {
        [JsonProperty("serviceKey")]
        public string ServiceKey { get; set; }
//        [JsonProperty("fields")]
//        public ICollection<Payment.Dtos.Field> Fields { get; set; }
        [JsonProperty("orderId")]
        public string OrderId { get; set; }
        //[JsonProperty("account")]
      //  public string Account { get; set; }
        [JsonProperty("description")] 
        public string Description { get; set; }
        [JsonProperty("amount")] 
        public double Amount { get; set; }
        //[JsonProperty("expire")] 
        //public DateTime? Expire { get; set; }
        //[JsonProperty("isOneTimePay")]
        //public bool? IsOneTimePay { get; set; }
    }
    
    
    public class Order
    {
        [JsonProperty("serviceKey")]
        public string ServiceKey { get; set; }
        [JsonProperty("fields")]
        public ICollection<Payment.Dtos.Field> Fields { get; set; }
        [JsonProperty("orderId")]
        public string OrderId { get; set; }
        [JsonProperty("account")]
        public string Account { get; set; }
        [JsonProperty("description")] 
        public string Description { get; set; }
        [JsonProperty("amount")] 
        public double Amount { get; set; }
        [JsonProperty("expire")] 
        public DateTime? Expire { get; set; }
        [JsonProperty("isOneTimePay")]
        public bool? IsOneTimePay { get; set; }
    }

    public class UserPaymentInstrument
    {
        [JsonProperty("instrumentId")] 
        public int InstrumentId { get; set; }
        [JsonProperty("instrumentType")] 
        public InstrumentType InstrumentType { get; set; }
    }
    
    public class UserInfo
    {
        [JsonProperty("phone")] 
        public string Phone { get; set; }
    }

    public class Urls
    {
        [JsonProperty("succes")] 
        public string Success { get; set; }
        [JsonProperty("failed")] 
        public string Failed { get; set; }
    }

    public class Recurrent
    {
        [JsonProperty("cronRule")] 
        public string CronRule { get; set; }
        [JsonProperty("dateExpire")] 
        public DateTime? DateExpire { get; set; }
    }

    public class Splitting
    {
        [JsonProperty("items")] 
        public ICollection<SplitItem> Items { get; set; }
    }

    public class SplitItem
    {
        [JsonProperty("bankingDetailsId")]
        public string BankingDetailsId { get; set; }
        [JsonProperty("bankingDetails")]
        public BankingDetails BankingDetails { get; set; }
        [JsonProperty("unit")]
        public Unit Unit { get; set; }
        [JsonProperty("value")]
        public double Value { get; set; }
    }
    
    [JsonConverter(typeof(StringEnumConverter), false)]
    public enum Unit
    {
        Amount,
        Percent
    }
    
    public class Field
    {
        [JsonProperty("fieldName")] 
        public string FieldName { get; set; }
        [JsonProperty("fieldValue")] 
        public string FieldValue { get; set; }
        [JsonProperty("fieldKey")] 
        public string FieldKey { get; set; }
    }
}