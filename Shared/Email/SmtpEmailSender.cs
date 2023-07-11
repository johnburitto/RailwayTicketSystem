using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;

namespace Shared.Email
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpClient _smtpClient;

        public SmtpEmailSender(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient ?? throw new ArgumentNullException(nameof(smtpClient));
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MailMessage()
            {
                From = new MailAddress("railwayticketssystem@gmail.com"),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            message.To.Add(email);

            await _smtpClient.SendMailAsync(message);
        }

        public async Task SendEmailWithAttachedImageAsync(string email, string subject, string htmlMessage, byte[] img)
        {
            var message = new MailMessage()
            {
                From = new MailAddress("railwayticketssystem@gmail.com"),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            Attachment data = new Attachment(new MemoryStream(img), "qr.png");

            data.ContentId = "qr.png";
            data.ContentDisposition.Inline = true;
            message.To.Add(email);
            message.Attachments.Add(data);

            await _smtpClient.SendMailAsync(message);
        }
    }
}
