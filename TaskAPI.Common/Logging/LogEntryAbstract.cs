using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskAPI.Common.Logging
{
    public abstract class LogEntryAbstract
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            });
        }

        protected string GetHostName() { 
            return string.IsNullOrEmpty(Environment.GetEnvironmentVariable("COMPUTERNAME"))
                ? Environment.GetEnvironmentVariable("HOSTNAME")
                : Environment.GetEnvironmentVariable("COMPUTERNAME");
        }
    }
}
