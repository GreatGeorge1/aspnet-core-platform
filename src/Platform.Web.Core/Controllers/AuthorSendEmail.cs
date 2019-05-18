using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Controllers;
using Abp.BackgroundJobs;
using Abp.Net.Mail;
using Abp.Net.Mail.Smtp;
using JetBrains.Annotations;
using Platform.Background;
using Microsoft.AspNetCore.Mvc;

namespace Platform.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SendEmail: AbpController
    {
        [NotNull] private readonly IBackgroundJobManager _backgroundJobManager;
        public SendEmail([NotNull] IBackgroundJobManager backgroundJobManager)
        {
            _backgroundJobManager =
                backgroundJobManager ?? throw new ArgumentNullException(nameof(backgroundJobManager));
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
                                Вид діяльності <b>{input.Profession}</b><br><br>
                                Місце роботи <b>{input.Company}</b><br><br>
                                Про мене: <b>{input.Description}</b><br><br>
                                Файли  {files}"
                });
        }
        
        [HttpPost]
        public async Task SendPsycho([FromBody]PsychoSendEmailDto input){
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
    }
    
    public class PsychoSendEmailDto{
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Package { get; set; }
    }
    
}