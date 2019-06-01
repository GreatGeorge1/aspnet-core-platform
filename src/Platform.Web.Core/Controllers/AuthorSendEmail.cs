using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Controllers;
using Abp.BackgroundJobs;
using Abp.Net.Mail;
using Abp.Net.Mail.Smtp;
using Abp.UI;
using JetBrains.Annotations;
using Platform.Background;
using Microsoft.AspNetCore.Mvc;
using Platform.Authorization.Users;
using Platform.Models.Google;
using Platform.Orders;
using Platform.Payment;
using Platform.Payment.Dtos;
using User = Platform.Authorization.Users.User;

namespace Platform.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SendEmail: AbpController
    {
        [NotNull] private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly EasyPayService _paymentService;
        public SendEmail([NotNull] IBackgroundJobManager backgroundJobManager,
            EasyPayService paymentService)
        {
            _backgroundJobManager =
                backgroundJobManager ?? throw new ArgumentNullException(nameof(backgroundJobManager));
            _paymentService= paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        }

        [HttpPost]
        public async Task SendAuthor([FromBody]AuthorSendEmailDto input){
            _ = await _backgroundJobManager.EnqueueAsync<SendEMailJob, SendEmailArgs>(
            new SendEmailArgs
            {
                Email = input.Email,
                Subject = "Реєстрація - choizy.org",
                isHtml = true,
                Message = @"<b>Вітаємо з реєстрацією на профорієнтаційній платформі ChoiZY!</b><br><br>
                                Незабаром ми з Вами зв’яжемось для обговорення деталей співпраці!<br><br>
                                Виникли питання? Звертайтесь до нас: <a href = 'mailto: shmakova@choizy.org'>shmakova@choizy.org</a>"
            });


            string files = "";
            foreach (var item in input.FileUrls)
            {
                files += $"<b><a href='{item}'>{item}</a></b><br>";
            }
            _ = await _backgroundJobManager.EnqueueAsync<SendEMailJob, SendEmailArgs>(
                new SendEmailArgs
                {
                    Email = "info@choizy.org",
                    Subject = $"Реєстрація викладач - {input.Email}",
                    isHtml = true,
                    Message = $@"Ім'я: <b>{input.Name}</b><br><br>
                                Email: <a href = 'mailto: {input.Email}'>{input.Email}</a><br><br>
                                Досвід викладача <b>{(input.DosvidVykl?"Так":"Ні")}</b><br><br>
                                Досвід зйомки відео <b>{(input.DosvidVideo?"Так":"Ні")}</b><br><br>
                                Телефон: <b>{input.Phone}</b><br><br>
                                Вид діяльності <b>{input.Profession}</b><br><br>
                                Місце роботи <b>{input.Company}</b><br><br>
                                Про мене: <b>{input.Description}</b><br><br>
                                Файли  {files}"
                });
        }
        
        [HttpPost]
        public async Task<ConfirmOrderResponseDto> SendPsycho([FromBody]PsychoSendEmailDto input)
        {

           // var user = await _userManager.FindByEmailAsync(input.Email);
           ConfirmOrderResponseDto res=new ConfirmOrderResponseDto();
            switch (input.PackageId)
            {
                case 1:
                    //550
                    res= await this.GenOrder(550, input.Phone, input.Name, input.Email, input.Package);
                    break;
                case 2:
                    //550
                    res= await this.GenOrder(550, input.Phone, input.Name, input.Email, input.Package);
                    break;
                case 3:
                    //950
                    res= await this.GenOrder(950, input.Phone, input.Name, input.Email, input.Package);
                    break;
                case 4:
                    //1350
                    res= await this.GenOrder(1350, input.Phone, input.Name, input.Email, input.Package);
                    break;
                default:
                    throw new UserFriendlyException("такого id не существует");
            }
            _ = await _backgroundJobManager.EnqueueAsync<SendEMailJob, SendEmailArgs>(
                new SendEmailArgs
                {
                    Email = input.Email,
                    Subject = "Реєстрація на консультацію психолога - choizy.org",
                    isHtml = true,
                    Message = @"<b>Вітаємо з реєстрацією на психологічне тестування та консультацію!</b><br><br>
                                Незабаром ми з Вами зв‘яжемось!<br><br>
                                Виникли питання? Звертайтесь до нас:  <a href = 'mailto: info@choizy.org'>info@choizy.org</a>"
                });
            
            _ = await _backgroundJobManager.EnqueueAsync<SendEMailJob, SendEmailArgs>(
                new SendEmailArgs
                {
                    Email = "info@choizy.org",
                    Subject = $"Реєстрація психолог - {input.Email}",
                    isHtml = true,
                    Message = $@"Ім'я: <b>{input.Name}</b><br><br>
                                Email: <a href = 'mailto: {input.Email}'>{input.Email}</a><br><br>
                                Телефон: <b>{input.Phone}</b><br><br>
                                Послуга: <b>{input.Package}</b><br><br>"
                });
            return res;
        }


        private async Task<ConfirmOrderResponseDto> GenOrder(int summ, string phone, string name, string email, string desc)
        {
            var app=await _paymentService.CreateApp();

            var secret = UrlToken.GenerateToken();
            //var secret2 = UrlToken.GenerateToken();
            //order.SetData("secretSuccess",secret);
            // order.SetData("secretFailed",secret);
            try
            {
                var createOrder = await _paymentService.CreateOrder(app, secret, $"Оплата консультації психолога #{desc}. {name}, {email}, {phone}.",
                    (double) summ, new Urls()
                    {
                        //Success = $"{baseUrl}/?orderId={OrderId}&success=true&secret={secret}",
                        //Failed = $"{baseUrl}/?orderId={OrderId}&success=false&secret=null"
                    });
                return new ConfirmOrderResponseDto()
                {
                    ForwardUrl = createOrder.ForwardUrl,
                    Amount = createOrder.Amount
                };
            }
            catch (HttpRequestException e)
            {
                //await orderRepository.DeleteAsync(OrderId);
                throw new UserFriendlyException(e.Message);
            }
        }
        
        [HttpPost]
        public async Task SendFeedback([FromBody]FeedbackSendEmailDto input){
            _ = await _backgroundJobManager.EnqueueAsync<SendEMailJob, SendEmailArgs>(
                new SendEmailArgs
                {
                    Email = "info@choizy.org",
                    Subject = $"Фідбек - {input.Email}",
                    isHtml = true,
                    Message = $@"Ім'я: <b>{input.Name}</b><br><br>
                                Email: <a href = 'mailto: {input.Email}'>{input.Email}</a><br><br>
                                Телефон: <b>{input.Phone}</b><br><br>
                                Текст: <b>{input.Text}</b><br><br>"
                });
        }
    }
    
    public class AuthorSendEmailDto{
        public string Email { get; set; }
        public string Name { get; set; }
        public bool DosvidVykl { get; set; }
        public bool DosvidVideo { get; set; }
        public string Description { get; set; }
        public string Profession { get; set; }
        public string Company { get; set; }
        public ICollection<string> FileUrls { get; set; }
        public string Phone { get; set; }
    }
    
    public class PsychoSendEmailDto{
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Package { get; set; }
        public int PackageId { get; set; }
    }
    
    public class FeedbackSendEmailDto{
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Text { get; set; }
    }
    
}