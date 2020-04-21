using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskAPI.Services.Models.Inbound
{
    public class CreateTaskRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("target_date")]
        public DateTime TargetDate { get; set; }
        [JsonProperty("priority")]
        public int Priority { get; set; }
        [JsonProperty("assign_to")]
        public List<Guid> AssignTo { get; set; }
    }
}
