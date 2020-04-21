using System;
using System.Collections.Generic;
using System.Text;
using TaskAPI.Common.Enums;

namespace TaskAPI.Data.Models
{
    public class UpdateTask
    {
        public string Name { get; set; }
        public DateTime TargetDate { get; set; }
        public Priority Priority { get; set; }
    }
}
