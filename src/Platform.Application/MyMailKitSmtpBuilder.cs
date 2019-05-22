using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.MailKit;
using Abp.Net.Mail;
using Abp.Net.Mail.Smtp;
using Google.Apis.Auth.OAuth2;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Platform
{
    public class MyEmailSender : IEmailSender, ISingletonDependency
    {
        public MyEmailSender()
        {

        }

        private SmtpClient client;

        public async Task SendAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            var res = true;
            if (res)
            {
                using (var client = new SmtpClient ()) {
                    client.Connect ("smtp.gmail.com", 587);
                    
                    
                    client.Authenticate(Encoding.UTF8, "info@choizy.org", "kDGsgs19sdfkDGsgs19sdf");
                    
                    var message = new MimeMessage();
                    
                    message.From.Add(new MailboxAddress("Choizy.Org", "info@choizy.org"));
                    message.To.Add(new MailboxAddress(to, to));
                    message.Subject = subject;
                    if (isBodyHtml)
                    {
                        message.Body = new TextPart("html")
                        {
                            Text = body
                        };
                    }
                    else
                    {
                        message.Body = new TextPart("plain")
                        {
                            Text = body
                        };
                    }

                    client.Send (message);
                    client.Disconnect (true);
                }
                
            }
        }

        public void Send(string to, string subject, string body, bool isBodyHtml = true)
        {
            var res = true;
            if (res)
            {
                using (var client = new SmtpClient ()) {
                    client.Connect ("smtp.gmail.com", 587);
                    
                    
                    client.Authenticate(Encoding.UTF8, "info@choizy.org", "kDGsgs19sdfkDGsgs19sdf");
                    
                    var message = new MimeMessage();
                    
                    message.From.Add(new MailboxAddress("Choizy.Org", "info@choizy.org"));
                    message.To.Add(new MailboxAddress(to, to));
                    message.Subject = subject;
                    if (isBodyHtml)
                    {
                        message.Body = new TextPart("html")
                        {
                            Text = body
                        };
                    }
                    else
                    {
                        message.Body = new TextPart("plain")
                        {
                            Text = body
                        };
                    }

                    client.Send (message);
                    client.Disconnect (true);
                }
                
            }
        }

        public Task SendAsync(string @from, string to, string subject, string body, bool isBodyHtml = true)
        {
            throw new System.NotImplementedException();
        }

        public void Send(string @from, string to, string subject, string body, bool isBodyHtml = true)
        {
            throw new System.NotImplementedException();
        }

        public void Send(MailMessage mail, bool normalize = true)
        {
            throw new System.NotImplementedException();
        }

        public Task SendAsync(MailMessage mail, bool normalize = true)
        {
            throw new System.NotImplementedException();
        }
    }
}