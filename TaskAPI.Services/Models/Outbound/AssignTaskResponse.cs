using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskAPI.Services.Models.Outbound
{
    public class AssignTaskResponse
    {
        [JsonProperty("users_assigned")]
        public List<Guid> UsersAssigned { get; set; }
        [JsonProperty("users_not_assigned")]
        public List<Guid> UsersNotAssigned { get; set; }
    }
}
