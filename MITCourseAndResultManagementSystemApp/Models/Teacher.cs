using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.Models
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Name")][Display(Name = "Saffix")]
        public string suffix { get; set; }
        [Required(ErrorMessage = "Please Enter Name")][Display(Name = "First Name")]
        public string TeacherFName { get; set; }
        [Required(ErrorMessage = "Please Enter Name")][Display(Name = "Last Name")]
        public string TeacherLName { get; set; }

        [Display(Name = "Teacher Name")]
        public string TeacherName { get; set; }

        [Required(ErrorMessage = "Please Enter Address")][Display(Name = "Institute")]
        public string Institute { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter Contact Number")][Display(Name = "Contact Number")]
        public string ContactNo { get; set; }
        [Required(ErrorMessage = "Please Enter Designation")]
        public string Designation { get; set; }
        [Required(ErrorMessage = "Please Enter Credit")]
        public double CreditTaken { get; set; }

        public string PhotoPath { get; set; }
        [Required(ErrorMessage = "Please Enter Password")][DataType(DataType.Password)]
        public string Password { get; set; }
        [Required][Compare("Password", ErrorMessage = "Password Not matched")][DataType(DataType.Password),MinLength(8,ErrorMessage = "Please Enter Min 8 Char Password")]
        public string ConfirmPassword { get; set; }

       }
}