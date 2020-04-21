using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskAPI.Services.Models.Inbound
{
    public class UpdateTaskRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("target_date")]
        public DateTime TargetDate { get; set; }
        [JsonProperty("priority")]
        public int Priority { get; set; }
    }
}
