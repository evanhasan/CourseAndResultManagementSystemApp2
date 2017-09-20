using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace MITCourseAndResultManagementSystemApp.Models
{
    public class UnassignCourse
    {
        public int Id { get; set; }
        [Display(Name = "Batch")]
        public int  BatchId { get; set; }
        public int  Semester { get; set; }
        [Display(Name = "Course Name")]
        public int  DepartmentId { get; set; }
        public virtual Department   Departments{ get; set; }
    }
}