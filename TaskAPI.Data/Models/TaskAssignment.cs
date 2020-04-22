using System;
using System.Collections.Generic;
using System.Text;

namespace TaskAPI.Data.Models
{
    public class TaskAssignment
    {
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }

        public virtual User User { get; set; }
        public virtual UserTask Task { get; set; }
    }
}
