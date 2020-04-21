using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskAPI.Services.Models.Inbound
{
    public class CreateUserRequest
    {
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("sex")]
        public int Sex { get; set; }
    }
}
