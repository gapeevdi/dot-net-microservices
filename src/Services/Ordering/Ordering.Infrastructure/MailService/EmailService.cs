using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Ordering.Infrastructure.MailService
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> settings, ILogger<EmailService> logger)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> SendEmail(Email email)
        {
            var client = new SendGridClient(_settings.ApiKey);

            var to = new EmailAddress(email.To);
            var from = new EmailAddress
            {
                Email = _settings.FromAddress,
                Name = _settings.FromName
            };

            var request = MailHelper.CreateSingleEmail(from, to, email.Subject, email.Body, null);
            var response = await client.SendEmailAsync(request);

            _logger.LogInformation("Email sent.");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            _logger.LogInformation("Email sending failed");

            return false;
        }

    }
}
