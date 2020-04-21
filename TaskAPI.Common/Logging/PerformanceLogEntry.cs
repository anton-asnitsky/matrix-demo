using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskAPI.Common.Logging
{
    public class PerformanceLogEntry : LogEntryAbstract
    {
        public long Duration { get; }
        public string Path { get; }
        public string Hostname { get; }

        public PerformanceLogEntry(long duration, HttpContext context = null)
        {
            Duration = duration;
            Hostname = GetHostName();
            Path = context?.Request.Path;
        }

    }
}
