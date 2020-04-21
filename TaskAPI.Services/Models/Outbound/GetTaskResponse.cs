using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskAPI.Services.Models.Outbound
{
    public class GetTaskResponse
    {
        [JsonProperty("task_name")]
        public string Name { get; set; }
        [JsonProperty("task_priority")]
        public int Priority { get; set; }
        [JsonProperty("target_date")]
        public DateTime TargetDate { get; set; }
        [JsonProperty("task_completed")]
        public bool Done { get; set; }
        [JsonProperty("users_assigned")]
        public List<GetUserResponse> UsersAssigned { get; set; }
    }
}
