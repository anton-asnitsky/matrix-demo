using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskAPI.Services.Models.Outbound
{
    public class CreateTaskResponse
    {
        [JsonProperty("task_id")]
        public Guid TaskId { get; set; }
    }
}
