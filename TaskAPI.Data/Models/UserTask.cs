using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TaskAPI.Common.Enums;

namespace TaskAPI.Data.Models
{
    public class UserTask
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required, MaxLength(255)]
        public string Name { get; set; }
        [Required]
        public DateTime TargetDate { get; set; }
        [Required]
        public Priority Priority { get; set; }
        public Boolean Done { get; set; }

        /** Used for creating relationship between Users and Tasks tables **/
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
