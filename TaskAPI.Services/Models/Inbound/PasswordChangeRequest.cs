using System;
using System.Collections.Generic;
using System.Text;

namespace TaskAPI.Services.Models.Inbound
{
    public class PasswordChangeRequest
    {
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
