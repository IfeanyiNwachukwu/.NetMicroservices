using MimeKit;
using System.Linq;
using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfig _emailConfig;
        private IConfigurationRoot _configuration;
        public EmailSender(EmailConfig emailConfig)
        {
            var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _configuration = builder.Build();
            _emailConfig = emailConfig;

            //_firsemailBody = _rootConfiguration.ConfigurationRoot.GetValue<string>("FIRSmailBody");

            _emailConfig.From = _configuration.GetValue<string>("EmailConfiguration:From");
            _emailConfig.Username = _configuration.GetValue<string>("EmailConfiguration:Username");
            _emailConfig.Password = _configuration.GetValue<string>("EmailConfiguration:Password");
            _emailConfig.SmtpServer = _configuration.GetValue<string>("EmailConfiguration:SmtpServer");

        }
        public async Task SendEmailAsync(Message message)
        {
            var emailMesage = CreateEmailMessage(message);
            await SendAsync(emailMesage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
         

            var bodyBuilder = new BodyBuilder { HtmlBody = string.Format("<h2 style='color:red;'>{0}</h2>", message.Content) };
            if (message.Attachments != null && message.Attachments.Any())
            {
                int i = 1;
                foreach (var attachment in message.Attachments)
                {
                    bodyBuilder.Attachments.Add("attachment" + i, attachment);
                    i++;
                }
            }
            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        private async Task SendAsync(MimeMessage emailMesage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.Username, _emailConfig.Password);
                    await client.SendAsync(emailMesage);
                }
                catch (Exception ex)
                {
                    //This needs to be logged actually
                    Console.Out.WriteLine("" + ex.Message);
                    throw;

                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}
