﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TaskAPI.Common.Options
{
    public class MailerOptions
    {
        public string FromAddress { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }
    }
}
