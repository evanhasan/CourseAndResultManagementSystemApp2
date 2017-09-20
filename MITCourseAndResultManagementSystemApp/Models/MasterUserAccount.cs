using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.Models
{
    public class MasterUserAccount
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Please Enter Your First Name"), Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please Enter Your First Name"), Display(Name = "Last Name")]
        public string LastName { get; set; }
        [EmailAddress(ErrorMessage = "Enter Correct Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter Your Designation")]
        public string Designation { get; set; }
        [Required(ErrorMessage = "Please Enter Your Phone Number"), Display(Name = "Contact")]
        public string PhoneNo { get; set; }
        [Required(ErrorMessage = "Please Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Password Not matched")]
        [DataType(DataType.Password), Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}