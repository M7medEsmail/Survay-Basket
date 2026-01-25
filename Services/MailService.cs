using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using SurvayBacket.Api.Settings;

namespace SurvayBacket.Api.Services
{
    public class MailService(IOptions<MailSettings> mailSetting , ILogger<MailService> logger) : IEmailSender
    {
        private readonly MailSettings _mailSetting = mailSetting.Value;
        private readonly ILogger<MailService> _logger = logger;

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSetting.Mail),
                Subject = subject
            };
             
            message.To.Add(MailboxAddress.Parse(email));
            var builder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };
            message.Body = builder.ToMessageBody();

            _logger.LogInformation("Sending email to {Email} with subject {Subject}", email, subject);

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSetting.Host, _mailSetting.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSetting.Mail, _mailSetting.Password);
            smtp.Send(message);
            smtp.Disconnect(true);

        }
    }
}
