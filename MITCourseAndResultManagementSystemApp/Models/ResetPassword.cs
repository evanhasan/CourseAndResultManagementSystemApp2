using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.Models
{
    public class ResetPassword
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string RandomNumber { get; set; }
        public string RandomNumber2 { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
         [Required]
        public int AccountId { get; set; }
        [Required, DataType(DataType.EmailAddress), Display(Name = "Email")]
        public string Email { get; set; }

        public string UserIP { get; set; }
    }
}