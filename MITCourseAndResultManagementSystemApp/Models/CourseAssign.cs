using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.Models
{
    public class CourseAssign
    {
        [Key]
        public int Id { get; set; }

        [Required, Display(Name = "Batch")]
        public int BatchId { get; set; }
        [Required, Display(Name = "Subject")]
        public int CourseId { get; set; }
        [Required,  Display(Name = "Teacher")]
        public int TeacherId { get; set; }
        public double Credit { get; set; }
        public int Flag { get; set; }
        [Display(Name = "Course Name")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
    }
}