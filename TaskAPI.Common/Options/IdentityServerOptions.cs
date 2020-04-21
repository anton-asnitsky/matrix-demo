using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskAPI.Common.Options
{
    public class IdentityServerOptions
    {
        public string Host { get; set; }
        public bool RequireHttps { get; set; }
        public string CertFolder { get; set; }
        public string CertFile { get; set; }
        public string CertPassword { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
    }
}
