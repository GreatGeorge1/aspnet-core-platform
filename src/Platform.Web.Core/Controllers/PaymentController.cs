using System;
using System.IO;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Controllers;
using Abp.BackgroundJobs;
using Castle.Core.Internal;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using Platform.Background;
using Platform.Orders;
using Platform.Payment;
using Platform.Payment.Models;

namespace Platform.Controllers
{  
    [Route("api/[controller]/[action]")]
    public class PaymentController:AbpController
    {
        [NotNull] private readonly IBackgroundJobManager _backgroundJobManager;

        public PaymentController([NotNull] IBackgroundJobManager backgroundJobManager)
        {
            _backgroundJobManager =
                backgroundJobManager ?? throw new ArgumentNullException(nameof(backgroundJobManager));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EasyPayNotify([FromBody]EasyPayNotify input)
        {
            var sign = HttpContext.Request.Headers["Sign"];
            if (sign.IsNullOrEmpty())
            {
                return BadRequest();
            }
            
            using (var mem = new MemoryStream())
            using (var reader = new StreamReader(mem))
            {
                Request.Body.CopyTo(mem);
 
                var body = reader.ReadToEnd();
 
                // Do something
                _ = await _backgroundJobManager.EnqueueAsync<EasyPayProcessNotifyJob, EasyPayProcessNotifyArgs>(
                    new EasyPayProcessNotifyArgs
                    {
                        Sign = sign,
                        Body=body,
                        Notify = input
                    });
                
                mem.Seek(0, SeekOrigin.Begin);
 
                body = reader.ReadToEnd();
            }
            return Ok();
        }
    }
}