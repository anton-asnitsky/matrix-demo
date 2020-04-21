using System;
using System.Collections.Generic;
using System.Text;

namespace TaskAPI.Data.Models
{
    public class TaskAssignment
    {
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }

        public User User { get; set; }
        public UserTask Task { get; set; }
    }
}
