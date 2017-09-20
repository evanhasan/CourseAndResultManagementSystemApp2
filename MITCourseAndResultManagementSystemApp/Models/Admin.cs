using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.Models
{
    public class Admin
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Please Enter Your First Name"),Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please Enter Your First Name"), Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required,EmailAddress(ErrorMessage = "Enter Correct Email Address"), Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter Your Designation")]
        public string Designation { get; set; }
        [Required(ErrorMessage = "Please Enter Your Phone Number"), Display(Name = "Contact")]
        public string PhoneNo { get; set; }
        [Required(ErrorMessage = "Please Enter Power of Access"), Display(Name = "Privilege")]
        public int Power { get; set; }
        [Display(Name = "Photo")]
        public string PhotoPath { get; set; }

        [Required(ErrorMessage = "Please Enter Password"), Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Password Not matched"), Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}