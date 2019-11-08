using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace EFCore.Utilities.SendGrid
{
    public class EmailSender : IEmailSender
    {
        private SendGridSettings _settings;

        //Limitation : Send up to 40,000 emails for 30 days, then send 100 emails/day free forever
        //private readonly SendGridSettings _settings;
        public EmailSender(IOptions<SendGridSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<HttpStatusCode> SendEmailAsync(string recipientEmail, string subject, string plainText, string htmlContent)
        {
            var message = MailHelper.CreateSingleEmail
            (
                new EmailAddress(_settings.SenderEmail),
                new EmailAddress(recipientEmail),
                subject,
                plainText,
                htmlContent
            );

            var response = await new SendGridClient(_settings.APIKey)
                .SendEmailAsync(message);
            return response.StatusCode;
        }

        public async Task<HttpStatusCode> SendEmailAsync(string senderEmail, string recipientEmail, string subject, string plainText, string htmlContent)
        {
            var message = MailHelper.CreateSingleEmail
                (
                    new EmailAddress(senderEmail),
                    new EmailAddress(recipientEmail),
                    subject,
                    plainText,
                    htmlContent
                );

            var response = await new SendGridClient(_settings.APIKey)
                .SendEmailAsync(message);

            return response.StatusCode;
        }
    }
}
