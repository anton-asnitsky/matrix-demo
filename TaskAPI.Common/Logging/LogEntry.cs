using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskAPI.Common.Logging
{
    public class LogEntry : LogEntryAbstract
    {
        public string Message { get; }
        public string Hostname { get; }

        public LogEntry(string message)
        {
            Message = message;
            Hostname = GetHostName();
        }
    }
}
