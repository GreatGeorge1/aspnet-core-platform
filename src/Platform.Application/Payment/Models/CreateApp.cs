using System.Collections.Generic;
using Newtonsoft.Json;

namespace Platform.Payment.Models
{
    public class CreateApp
    {
        [JsonProperty("logoPath")]
        public string LogoPath { get; set; }
        [JsonProperty("hintImagesPath")]
        public string HintImagesPath { get; set; }
        [JsonProperty("apiVersion")]
        public string ApiVersion { get; set; }
        [JsonProperty("appId")]
        public string AppId { get; set; }
        [JsonProperty("pageId")]
        public string PageId { get; set; }
        [JsonProperty("error")]
        public Error Error { get; set; }
        [JsonProperty("requestedSessionId")]
        public string RequestedSessionId { get; set; }
    }

    public class Error
    {
        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }
        [JsonProperty("fieldError")]
        public ICollection<FieldError> FieldErrors { get; set; }
    }

    public class FieldError
    {
        [JsonProperty("fieldName")]
        public string FieldName { get; set; }
        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }
        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}