using System;
using System.Collections.Generic;
using System.Text;

namespace TaskAPI.Common.Options
{
    public class MailerOptions
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
