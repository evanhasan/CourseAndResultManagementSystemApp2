using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.Models
{
    public class Student
    {
        [Key]
        public int  Id { get; set; }
        [Required(ErrorMessage = "Please Enter Name"), Display(Name = "Student Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Enter Email"), DataType(DataType.EmailAddress), Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter Mobile Number"), Display(Name = "Contact")]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "Please Enter Address"), Display(Name = "Parmanent Address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Please Enter Gender")]
        public string Gender { get; set; }
      [Display(Name = "Registration No")]
        public string RegNo { get; set; }
        [Required(ErrorMessage = "Please Enter Roll Number"), Display(Name = "Roll Number")]
        public string RollNumber { get; set; }
        [ Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }
        [Required(ErrorMessage = "Please Enter Password"),DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password"), Required, DataType(DataType.Password), Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Batch")]
        public int BatchId { get; set; }
        [Display(Name = "Photo")]
        public string PhotoPath { get; set; }
        [Display(Name = "Course Name")]
        public int DepartmentId { get; set; }
        public virtual Department Departments { get; set; }
    }
}