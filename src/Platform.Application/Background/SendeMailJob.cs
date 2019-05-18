using System;
using System.Collections.Generic;
using System.Net;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Net.Mail;
using Abp.Net.Mail.Smtp;
using JetBrains.Annotations;

namespace Platform.Background
{
    public class SendEMailJob:BackgroundJob<SendEmailArgs>, ITransientDependency
    {
        [NotNull] private readonly MyEmailSender _emailSender;

        public SendEMailJob([NotNull] MyEmailSender emailSender)
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        public override void Execute(SendEmailArgs args)
        {
            _emailSender.Send(
                to:args.Email,
                // from:"info@choizy.org",
                subject: args.Subject,
                body: args.Message,
                isBodyHtml:args.isHtml
            );
        }
    }

    public class SendEmailArgs
    {
        public string Email { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public bool isHtml { get; set; }
    }
    
}