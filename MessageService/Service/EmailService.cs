using MailKit.Net.Smtp;
using MailKit.Security;
using MessageService.Interface;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace MessageService.Service
{
    public class EmailService : IMessageService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(
            string to,
            string subject,
            string body)
        {
            var host = _configuration["EmailSettings:Host"]!;
            var port = int.Parse(
                _configuration["EmailSettings:Port"]!);

            var username =
                _configuration["EmailSettings:Username"]!;

            var password =
                _configuration["EmailSettings:Password"]!;

            var message = new MimeMessage();

            message.From.Add(
                new MailboxAddress("Fundoo", username));

            message.To.Add(
                new MailboxAddress("", to));

            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using var client = new SmtpClient();

            client.CheckCertificateRevocation = false;

            await client.ConnectAsync(
                host,
                port,
                SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(
                username,
                password);

            await client.SendAsync(message);

            await client.DisconnectAsync(true);
        }
    }
}