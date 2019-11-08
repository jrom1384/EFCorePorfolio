using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Utilities.SendGrid
{
    public interface IEmailSender
    {
        Task<HttpStatusCode> SendEmailAsync(string recipientEmail, string subject, string plainText, string htmlContent);

        Task<HttpStatusCode> SendEmailAsync(string senderEmail, string recipientEmail, string subject, string plainText, string htmlContent);
    }
}
