using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.DTO
{
    public class EmailDTO
    {
        public string Recipient { get; set; }

        public string Subject { get; set; }

        public string HtmlContent { get; set; }
    }
}
