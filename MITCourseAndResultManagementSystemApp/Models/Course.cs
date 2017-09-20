using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.Models
{
    public class Course
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Code must have to be atleast 5 characture"), Display(Name = "SubJect Code")]
        public string CourseCode { get; set; }
        [Required(ErrorMessage = "Please Enter Course Name"), Display(Name = "SubJect Name")]
        public string CourseName { get; set; }
        [Required(ErrorMessage = "Please Enter Course Credit")]
        public double Credit { get; set; }
        [Required(ErrorMessage = "Please Enter Course CoCrDescription")]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Please Enter Course Semester")]
        public int Semester { get; set; }
        [Display(Name = "Course Name")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
    }
}