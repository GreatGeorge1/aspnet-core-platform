using System;
using Abp.AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Platform.Authentication.External;

namespace Platform.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo),typeof(AuthenticationScheme))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
     
        //public string DisplayName { get; }

       
       // public Type HandlerType { get; }
    }
}
