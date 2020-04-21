using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskAPI.Services.Models.Inbound
{
    public class AssignTaskRequest
    {
        [JsonProperty("users_to_assign")]
        public List<Guid> UsersToAssign { get; set; }
        [JsonProperty("users_to_unassign", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public List<Guid> UsersToUnssign { get; set; }
    }
}
