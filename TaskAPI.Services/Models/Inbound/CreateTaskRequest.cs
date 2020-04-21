using System;
using System.Collections.Generic;
using System.Text;

namespace TaskAPI.Services.Models.Inbound
{
    public class CreateTaskRequest
    {
        public string Name { get; set; }
        public DateTime TargetDate { get; set; }
        public int Priority { get; set; }
        public List<Guid> AssignTo { get; set; }
    }
}
