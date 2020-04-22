using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TaskAPI.Common.Enums;

namespace TaskAPI.Data.Models
{
    public class User
    {
        [Key, Required]
        public Guid UserId { get; set; }
        [Required]
        public string Password { get; set; }
        [MaxLength(255), Required]
        public string FirstName { get; set; }
        [MaxLength(255), Required]
        public string LastName { get; set; }
        [MaxLength(20), Required]
        public string Phone { get; set; }
        [MaxLength(255), Required]
        public string Email { get; set; }
        public string JwtToken { get; set; }
        [MaxLength(233), Required]
        public string Address { get; set;  }
        [Required]
        public Sex Sex { get; set; }
        public string PasswordRecoveryToken { get; set; }
        public virtual ICollection<TaskAssignment> Assignments { get; set; }
    }
}
