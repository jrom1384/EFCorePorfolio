using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.Utilities.SendGrid
{
    public class SendGridSettings
    {
        public string APIKey { get; set; }
        public string SenderEmail { get; set; }
    }
}
