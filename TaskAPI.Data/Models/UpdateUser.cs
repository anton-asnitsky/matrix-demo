using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TaskAPI.Common.Enums;

namespace TaskAPI.Data.Models
{
    public class UpdateUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public Sex Sex { get; set; }
    }
}
