using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskAPI.Common.Logging
{
    public class RequestLogEntry : LogEntryAbstract
    {
        public int ErrorCode { get; }
        public string Message { get; }
        public string Path { get; }
        public string Hostname { get; }

        public RequestLogEntry(string message, int errorCode, HttpContext context)
        {
            Message = message;
            ErrorCode = errorCode;
            Hostname = GetHostName();
            Path = context.Request.Path;
        }
    }
}
