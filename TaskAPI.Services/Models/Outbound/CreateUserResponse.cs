using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskAPI.Services.Models.Outbound
{
    public class CreateUserResponse
    {
        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }
}
