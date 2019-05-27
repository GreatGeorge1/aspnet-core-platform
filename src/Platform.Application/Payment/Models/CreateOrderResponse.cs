using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Platform.Payment.Models
{
    public class CreateOrderResponse
    {
        //[JsonProperty("accountInfo")]
        //public string AccountInfo { get; set; }
        [JsonProperty("bankingDetails")]
        public string BankingDetails { get; set; }
        [JsonProperty("amountMin")] 
        public double? AmountMin { get; set; }
        [JsonProperty("amountMax")] 
        public double? AmountMax { get; set; }
        [JsonProperty("amount")] 
        public double Amount { get; set; }
        [JsonProperty("forwardUrl")] 
        public string ForwardUrl { get; set; }
        [JsonProperty("actionType")] 
        public ActionType ActionType { get; set; }
        [JsonProperty("transactionId")] 
        public int? TransactionId { get; set; }
        [JsonProperty("retrievalReferenceNo")] 
        public string RetrievalReferenceNo { get; set; }
        [JsonProperty("error")] 
        public Error Error { get; set; }
    }

    public class BankingDetails
    {
        [JsonProperty("payee")] 
        public Payee Payee { get; set; }
        [JsonProperty("payer")] 
        public Payer Payer { get; set; }
        [JsonProperty("narrative")] 
        public Narrative Narrative { get; set; }
    }

    public class Narrative
    {
        [JsonProperty("name")] 
        public string Name { get; set; }
    }
    public class Payee
    {
        [JsonProperty("id")] 
        public string Id { get; set; }
        [JsonProperty("name")] 
        public string Name { get; set; }
        [JsonProperty("bank")] 
        public Bank Bank { get; set; }
    }
    public class Payer
    {
        [JsonProperty("name")] 
        public string Name { get; set; }
    }
    public class Bank
    {
        [JsonProperty("name")] 
        public string Name { get; set; }
        [JsonProperty("mfo")]
        public string Mfo { get; set; }
        [JsonProperty("account")] 
        public string Account { get; set; }
    }
    
    public class PaymentInstrumentType
    {
        [JsonProperty("instrumentType")] 
        public InstrumentType InstrumentType { get; set; }
        [JsonProperty("commission")]
        public double? Comission { get; set; }
        [JsonProperty("amountMin")] 
        public double? AmountMin { get; set; }
        [JsonProperty("amountMax")] 
        public double?AmountMax { get; set; }
        [JsonProperty("userPaymentInstruments")] 
        public ICollection<PaymentInstrument> UserPaymentInstruments { get; set; }
    }

    public class PaymentInstrument
    {    
        [JsonProperty("instrumentId")]
        public int InstrumentId { get; set; }
        [JsonProperty("instrumentType")] 
        public InstrumentType InstrumentType { get; set; }
        [JsonProperty("instrumentValue")] 
        public string InstrumentValue { get; set; }
        [JsonProperty("alias")] 
        public string Alias { get; set; }
        [JsonProperty("commission")]
        public double? Comission { get; set; }
        [JsonProperty("loyaltyCommission ")]
        public double? LoyaltyCommission  { get; set; }
        [JsonProperty("actionsKeys")]
        public string[] ActionsKeys { get; set; } 
        [JsonProperty("priorityIndex")]
        public int PriorityIndex { get; set; }
      //  [JsonProperty("additionalParams")]
       // public object AdditionalParams { get; set; }
        
    }
    
    [JsonConverter(typeof(StringEnumConverter), false)]
    public enum InstrumentType
    {
        Emoney,
        RCard,
        Card,
        LiqPay,
        Terminal,
        KSMoney,
        EBank,
        LifeMoney,
        MasterPass,
        QrCode,
        FishkaB2B,
        VCard,
        QrMasterpass,
        FishkaB2C,
        VCash,
        CardRegistration
    }
    [JsonConverter(typeof(StringEnumConverter), false)]
    public enum MasterPassWalletStatus 
    {
        Unknown,
        NotRegistered,
        NeedLinkCardToCLient,
        AlreadyRegistered,
        UnknownStatus,
        NeedEasypayVerification
    }
    [JsonConverter(typeof(StringEnumConverter), false)]
    public enum ActionType
    {
        UrlRedirect,
        FormRedirect,
        PageRedirect,
        ConfirmCode
    }
}