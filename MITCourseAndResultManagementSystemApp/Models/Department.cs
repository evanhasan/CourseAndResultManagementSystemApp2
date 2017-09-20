using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.Models
{
    public class Department
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Course Code")]
        [StringLength(7, MinimumLength = 2, ErrorMessage = "Code Length must have to be between 2-7")]
        public string DepartmentCode { get; set; }
        [Required(ErrorMessage = "Please Enter Valide Name")]
        [Display(Name = "Course Name")]
        public string DepartmentName { get; set; }

        public virtual List<Course> Courses { get; set; } 
    }
}